﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.SessionState;


namespace JIC.ServicesJicCrimTests.Similator
{
    public enum HttpVerb
    {
        GET,
        HEAD,
        POST,
        PUT,
        DELETE,
    }

    /// <summary>
    /// Useful class for simulating the HttpContext. This does not actually 
    /// make an HttpRequest, it merely simulates the state that your code 
    /// would be in "as if" handling a request. Thus the HttpContext.Current 
    /// property is populated.
    /// http://haacked.com/archive/2007/06/19/unit-tests-web-code-without-a-web-server-using-httpsimulator.aspx
    /// </summary>
    public class HttpSimulator : IDisposable
    {
        private const string DefaultPhysicalAppPath = @"c:\InetPub\wwwRoot\";
        private readonly NameValueCollection headers = new NameValueCollection();
        private readonly NameValueCollection formVars = new NameValueCollection();
        private StringBuilder builder;
        private Uri referrer;
        private string page;

        public HttpSimulator()
            : this("/", DefaultPhysicalAppPath)
        {
        }

        public HttpSimulator(string applicationPath)
            : this(applicationPath, DefaultPhysicalAppPath)
        {
        }

        public HttpSimulator(string applicationPath, string physicalApplicationPath)
        {
            this.ApplicationPath = applicationPath;
            this.PhysicalApplicationPath = physicalApplicationPath;
        }

        /// <summary>
        /// Sets up the HttpContext objects to simulate a GET request.
        /// </summary>
        /// <remarks>
        /// Simulates a request to http://localhost/
        /// </remarks>
        public HttpSimulator SimulateRequest()
        {
            return SimulateRequest(new Uri("http://localhost/"));
        }

        /// <summary>
        /// Sets up the HttpContext objects to simulate a GET request.
        /// </summary>
        /// <param name="url"></param>
        public HttpSimulator SimulateRequest(Uri url)
        {
            return SimulateRequest(url, HttpVerb.GET);
        }

        /// <summary>
        /// Sets up the HttpContext objects to simulate a request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpVerb"></param>
        public HttpSimulator SimulateRequest(Uri url, HttpVerb httpVerb)
        {
            return SimulateRequest(url, httpVerb, null, null);
        }

        /// <summary>
        /// Sets up the HttpContext objects to simulate a POST request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formVariables"></param>
        public HttpSimulator SimulateRequest(Uri url, NameValueCollection formVariables)
        {
            return SimulateRequest(url, HttpVerb.POST, formVariables, null);
        }

        /// <summary>
        /// Sets up the HttpContext objects to simulate a POST request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formVariables"></param>
        /// <param name="headers"></param>
        public HttpSimulator SimulateRequest(Uri url, NameValueCollection formVariables, NameValueCollection headers)
        {
            return SimulateRequest(url, HttpVerb.POST, formVariables, headers);
        }

        /// <summary>
        /// Sets up the HttpContext objects to simulate a request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpVerb"></param>
        /// <param name="headers"></param>
        public HttpSimulator SimulateRequest(Uri url, HttpVerb httpVerb, NameValueCollection headers)
        {
            return SimulateRequest(url, httpVerb, null, headers);
        }

        /// <summary>
        /// Sets up the HttpContext objects to simulate a request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpVerb"></param>
        /// <param name="formVariables"></param>
        /// <param name="headers"></param>
        protected virtual HttpSimulator SimulateRequest(Uri url, HttpVerb httpVerb, NameValueCollection formVariables, NameValueCollection headers)
        {
            HttpContext.Current = null;

            ParseRequestUrl(url);

            if (this.responseWriter == null)
            {
                this.builder = new StringBuilder();
                this.responseWriter = new StringWriter(builder);
            }

            SetHttpRuntimeInternals();

            var query = ExtractQueryStringPart(url);

            if (formVariables != null)
            {
                formVars.Add(formVariables);
            }

            if (formVars.Count > 0)
            {
                httpVerb = HttpVerb.POST; //Need to enforce this.
            }

            if (headers != null)
            {
                this.headers.Add(headers);
            }

            this.workerRequest = new SimulatedHttpRequest(ApplicationPath, PhysicalApplicationPath, PhysicalPath, Page, query, this.responseWriter, host, port, httpVerb.ToString());

            this.workerRequest.Form.Add(formVars);
            this.workerRequest.Headers.Add(this.headers);

            if (referrer != null)
            {
                this.workerRequest.SetReferrer(referrer);
            }

            InitializeSession();

            InitializeApplication();

            #region Console Debug INfo

            Console.WriteLine("host: " + host);
            Console.WriteLine("virtualDir: " + applicationPath);
            Console.WriteLine("page: " + localPath);
            Console.WriteLine("pathPartAfterApplicationPart: " + page);
            Console.WriteLine("appPhysicalDir: " + physicalApplicationPath);
            Console.WriteLine("Request.Url.LocalPath: " + HttpContext.Current.Request.Url.LocalPath);
            Console.WriteLine("Request.Url.Host: " + HttpContext.Current.Request.Url.Host);
            Console.WriteLine("Request.FilePath: " + HttpContext.Current.Request.FilePath);
            Console.WriteLine("Request.Path: " + HttpContext.Current.Request.Path);
            Console.WriteLine("Request.RawUrl: " + HttpContext.Current.Request.RawUrl);
            Console.WriteLine("Request.Url: " + HttpContext.Current.Request.Url);
            Console.WriteLine("Request.Url.Port: " + HttpContext.Current.Request.Url.Port);
            Console.WriteLine("Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
            Console.WriteLine("Request.PhysicalPath: " + HttpContext.Current.Request.PhysicalPath);
            Console.WriteLine("HttpRuntime.AppDomainAppPath: " + HttpRuntime.AppDomainAppPath);
            Console.WriteLine("HttpRuntime.AppDomainAppVirtualPath: " + HttpRuntime.AppDomainAppVirtualPath);
            Console.WriteLine("HostingEnvironment.ApplicationPhysicalPath: " + HostingEnvironment.ApplicationPhysicalPath);
            Console.WriteLine("HostingEnvironment.ApplicationVirtualPath: " + HostingEnvironment.ApplicationVirtualPath);

            #endregion

            return this;
        }

        private static void InitializeApplication()
        {
            Type appFactoryType = Type.GetType("System.Web.HttpApplicationFactory, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            object appFactory = ReflectionHelper.GetStaticFieldValue<object>("_theApplicationFactory", appFactoryType);
            ReflectionHelper.SetPrivateInstanceFieldValue("_state", appFactory, HttpContext.Current.Application);
        }

        private void InitializeSession()
        {
            HttpContext.Current = new HttpContext(workerRequest);
            HttpContext.Current.Items.Clear();
            var session = (HttpSessionState)ReflectionHelper.Instantiate(typeof(HttpSessionState), new Type[] { typeof(IHttpSessionState) }, new FakeHttpSessionState());

            HttpContext.Current.Items.Add("AspSession", session);
        }

        public class FakeHttpSessionState : NameObjectCollectionBase, IHttpSessionState
        {
            private readonly bool isNewSession = true;
            private readonly string sessionID = Guid.NewGuid().ToString();
            private readonly HttpStaticObjectsCollection staticObjects = new HttpStaticObjectsCollection();
            private readonly object syncRoot = new Object();
            private int timeout = 30; //minutes
            private int lcid;
            private int codePage;

            /// <summary>
            /// Ends the current session.
            /// </summary>
            ///
            public void Abandon()
            {
                BaseClear();
            }

            /// <summary>
            /// Adds a new item to the session-state collection.
            /// </summary>
            /// <param name="name">The name of the item to add to the session-state collection. </param>
            /// <param name="value">The value of the item to add to the session-state collection. </param>
            public void Add(string name, object value)
            {
                BaseAdd(name, value);
            }

            /// <summary>
            /// Deletes an item from the session-state item collection.
            /// </summary>
            /// <param name="name">The name of the item to delete from the session-state item collection. </param>
            public void Remove(string name)
            {
                BaseRemove(name);
            }

            /// <summary>
            /// Deletes an item at a specified index from the session-state item collection.
            /// </summary>
            /// <param name="index">The index of the item to remove from the session-state collection. </param>
            public void RemoveAt(int index)
            {
                BaseRemoveAt(index);
            }

            /// <summary>
            /// Clears all values from the session-state item collection.
            /// </summary>
            public void Clear()
            {
                BaseClear();
            }

            /// <summary>
            /// Clears all values from the session-state item collection.
            /// </summary>
            public void RemoveAll()
            {
                BaseClear();
            }

            /// <summary>
            /// Copies the collection of session-state item values to a one-dimensional array, starting at the specified index in the array.
            /// </summary>
            /// <param name="array">The <see cref="T:System.Array"></see> that receives the session values. </param>
            /// <param name="index">The index in array where copying starts. </param>
            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets the unique session identifier for the session.
            /// </summary>
            /// <returns> The session ID. </returns>
            public string SessionID
            {
                get { return sessionID; }
            }

            /// <summary>
            /// Gets and sets the time-out period (in minutes) allowed between requests before the session-state provider terminates the session.
            /// </summary>
            /// <returns> The time-out period, in minutes. </returns>
            public int Timeout
            {
                get { return timeout; }
                set { timeout = value; }
            }

            /// <summary>
            /// Gets a value indicating whether the session was created with the current request.
            /// </summary>
            /// <returns> true if the session was created with the current request; otherwise, false. </returns>
            ///
            public bool IsNewSession
            {
                get { return isNewSession; }
            }

            /// <summary>
            /// Gets the current session-state mode.
            /// </summary>
            /// <returns> One of the <see cref="T:System.Web.SessionState.SessionStateMode"></see> values. </returns>
            public SessionStateMode Mode
            {
                get { return SessionStateMode.InProc; }
            }

            /// <summary>
            /// Gets a value indicating whether the session ID is embedded in the URL or stored in an HTTP cookie.
            /// </summary>
            ///
            /// <returns>
            /// true if the session is embedded in the URL; otherwise, false.
            /// </returns>
            ///
            public bool IsCookieless
            {
                get { return false; }
            }

            /// <summary>
            /// Gets a value that indicates whether the application is configured for cookieless sessions.
            /// </summary>
            ///
            /// <returns>
            /// One of the <see cref="T:System.Web.HttpCookieMode"></see> values that indicate whether the application is configured for cookieless sessions. The default is <see cref="F:System.Web.HttpCookieMode.UseCookies"></see>.
            /// </returns>
            ///
            public HttpCookieMode CookieMode
            {
                get { return HttpCookieMode.UseCookies; }
            }

            /// <summary>
            /// Gets or sets the locale identifier (LCID) of the current session.
            /// </summary>
            ///
            /// <returns>
            /// A <see cref="T:System.Globalization.CultureInfo"></see> instance that specifies the culture of the current session.
            /// </returns>
            ///
            public int LCID
            {
                get { return lcid; }
                set { lcid = value; }
            }

            /// <summary>
            /// Gets or sets the code-page identifier for the current session.
            /// </summary>
            ///
            /// <returns>
            /// The code-page identifier for the current session.
            /// </returns>
            ///
            public int CodePage
            {
                get { return codePage; }
                set { codePage = value; }
            }

            /// <summary>
            /// Gets a collection of objects declared by &lt;object Runat="Server" Scope="Session"/&gt; tags within the ASP.NET application file Global.asax.
            /// </summary>
            ///
            /// <returns>
            /// An <see cref="T:System.Web.HttpStaticObjectsCollection"></see> containing objects declared in the Global.asax file.
            /// </returns>
            ///
            public HttpStaticObjectsCollection StaticObjects
            {
                get { return staticObjects; }
            }

            /// <summary>
            /// Gets or sets a session-state item value by name.
            /// </summary>
            ///
            /// <returns>
            /// The session-state item value specified in the name parameter.
            /// </returns>
            ///
            /// <param name="name">The key name of the session-state item value. </param>
            public object this[string name]
            {
                get { return BaseGet(name); }
                set { BaseSet(name, value); }
            }

            /// <summary>
            /// Gets or sets a session-state item value by numerical index.
            /// </summary>
            ///
            /// <returns>
            /// The session-state item value specified in the index parameter.
            /// </returns>
            ///
            /// <param name="index">The numerical index of the session-state item value. </param>
            public object this[int index]
            {
                get { return BaseGet(index); }
                set { BaseSet(index, value); }
            }

            /// <summary>
            /// Gets an object that can be used to synchronize access to the collection of session-state values.
            /// </summary>
            ///
            /// <returns>
            /// An object that can be used to synchronize access to the collection.
            /// </returns>
            ///
            public object SyncRoot
            {
                get { return syncRoot; }
            }



            /// <summary>
            /// Gets a value indicating whether access to the collection of session-state values is synchronized (thread safe).
            /// </summary>
            /// <returns>
            /// true if access to the collection is synchronized (thread safe); otherwise, false.
            /// </returns>
            ///
            public bool IsSynchronized
            {
                get { return true; }
            }

            /// <summary>
            /// Gets a value indicating whether the session is read-only.
            /// </summary>
            ///
            /// <returns>
            /// true if the session is read-only; otherwise, false.
            /// </returns>
            ///
            bool IHttpSessionState.IsReadOnly
            {
                get
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Sets the referrer for the request. Uses a fluent interface.
        /// </summary>
        /// <param name="referer"></param>
        /// <returns></returns>
        public HttpSimulator SetReferer(Uri referer)
        {
            if (this.workerRequest != null)
            {
                this.workerRequest.SetReferrer(referer);
            }
            this.referrer = referer;
            return this;
        }

        /// <summary>
        /// Sets a form variable.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpSimulator SetFormVariable(string name, string value)
        {
            if (this.workerRequest != null)
            {
                throw new InvalidOperationException("Cannot set form variables after calling Simulate().");
            }

            formVars.Add(name, value);

            return this;
        }

        /// <summary>
        /// Sets a header value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpSimulator SetHeader(string name, string value)
        {
            if (this.workerRequest != null)
            {
                throw new InvalidOperationException("Cannot set headers after calling Simulate().");
            }

            headers.Add(name, value);

            return this;
        }

        private void ParseRequestUrl(Uri url)
        {
            if (url == null)
            {
                return;
            }
            this.host = url.Host;
            this.port = url.Port;
            this.localPath = url.LocalPath;
            this.page = StripPrecedingSlashes(RightAfter(url.LocalPath, ApplicationPath));
            this.physicalPath = Path.Combine(this.physicalApplicationPath, this.page.Replace("/", @"\"));
        }

        static string RightAfter(string original, string search)
        {
            if (search.Length > original.Length || search.Length == 0)
            {
                return original;
            }

            var searchIndex = original.IndexOf(search, 0, StringComparison.InvariantCultureIgnoreCase);

            if (searchIndex < 0)
            {
                return original;
            }

            return original.Substring(original.IndexOf(search) + search.Length);
        }

        public string Host
        {
            get { return this.host; }
        }

        private string host;

        public string LocalPath
        {
            get { return this.localPath; }
        }

        private string localPath;

        public int Port
        {
            get { return this.port; }
        }

        private int port;

        /// <summary>
        /// Portion of the URL after the application.
        /// </summary>
        public string Page
        {
            get { return this.page; }
        }

        /// <summary>
        /// The same thing as the IIS Virtual directory. It's 
        /// what gets returned by Request.ApplicationPath.
        /// </summary>
        public string ApplicationPath
        {
            get
            {
                return this.applicationPath;
            }
            set
            {
                this.applicationPath = value ?? "/";
                this.applicationPath = NormalizeSlashes(this.applicationPath);
            }
        }
        private string applicationPath = "/";

        /// <summary>
        /// Physical path to the application (used for simulation purposes).
        /// </summary>
        public string PhysicalApplicationPath
        {
            get
            {
                return this.physicalApplicationPath;
            }
            set
            {
                this.physicalApplicationPath = value ?? DefaultPhysicalAppPath;
                //strip trailing backslashes.
                this.physicalApplicationPath = StripTrailingBackSlashes(this.physicalApplicationPath) + @"\";
            }
        }

        private string physicalApplicationPath = DefaultPhysicalAppPath;

        /// <summary>
        /// Physical path to the requested file (used for simulation purposes).
        /// </summary>
        public string PhysicalPath
        {
            get { return this.physicalPath; }
        }

        private string physicalPath = DefaultPhysicalAppPath;

        public TextWriter ResponseWriter
        {
            get { return this.responseWriter; }
            set { this.responseWriter = value; }
        }

        /// <summary>
        /// Returns the text from the response to the simulated request.
        /// </summary>
        public string ResponseText
        {
            get
            {
                return (builder ?? new StringBuilder()).ToString();
            }
        }

        private TextWriter responseWriter;

        public SimulatedHttpRequest WorkerRequest
        {
            get { return this.workerRequest; }
        }

        private SimulatedHttpRequest workerRequest;

        private static string ExtractQueryStringPart(Uri url)
        {
            string query = url.Query ?? string.Empty;
            if (query.StartsWith("?"))
            {
                return query.Substring(1);
            }
            return query;
        }

        void SetHttpRuntimeInternals()
        {
            //We cheat by using reflection.

            // get singleton property value
            var runtime = ReflectionHelper.GetStaticFieldValue<HttpRuntime>("_theRuntime", typeof(HttpRuntime));

            // set app path property value
            ReflectionHelper.SetPrivateInstanceFieldValue("_appDomainAppPath", runtime, PhysicalApplicationPath);
            // set app virtual path property value
            const string VpathTypeName = "System.Web.VirtualPath, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
            var virtualPath = ReflectionHelper.Instantiate(VpathTypeName, new Type[] { typeof(string) }, new object[] { ApplicationPath });
            ReflectionHelper.SetPrivateInstanceFieldValue("_appDomainAppVPath", runtime, virtualPath);

            // set codegen dir property value
            ReflectionHelper.SetPrivateInstanceFieldValue("_codegenDir", runtime, PhysicalApplicationPath);

            HostingEnvironment environment = GetHostingEnvironment();
            ReflectionHelper.SetPrivateInstanceFieldValue("_appPhysicalPath", environment, PhysicalApplicationPath);
            ReflectionHelper.SetPrivateInstanceFieldValue("_appVirtualPath", environment, virtualPath);
            ReflectionHelper.SetPrivateInstanceFieldValue("_configMapPath", environment, new ConfigMapPath(this));
        }

        protected static HostingEnvironment GetHostingEnvironment()
        {
            HostingEnvironment environment;
            try
            {
                environment = new HostingEnvironment();
            }
            catch (InvalidOperationException)
            {
                //Shoot, we need to grab it via reflection.
                environment = ReflectionHelper.GetStaticFieldValue<HostingEnvironment>("_theHostingEnvironment", typeof(HostingEnvironment));
            }
            return environment;
        }

        #region --- Text Manipulation Methods for slashes ---
        protected static string NormalizeSlashes(string s)
        {
            if (String.IsNullOrEmpty(s) || s == "/")
            {
                return "/";
            }

            s = s.Replace(@"\", "/");

            //Reduce multiple slashes in row to single.
            var normalized = Regex.Replace(s, "(/)/+", "$1");
            //Strip left.
            normalized = StripPrecedingSlashes(normalized);
            //Strip right.
            normalized = StripTrailingSlashes(normalized);
            return "/" + normalized;
        }

        protected static string StripPrecedingSlashes(string s)
        {
            return Regex.Replace(s, "^/*(.*)", "$1");
        }

        protected static string StripTrailingSlashes(string s)
        {
            return Regex.Replace(s, "(.*)/*$", "$1", RegexOptions.RightToLeft);
        }

        protected static string StripTrailingBackSlashes(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return Regex.Replace(s, @"(.*)\\*$", "$1", RegexOptions.RightToLeft);
        }
        #endregion

        internal class ConfigMapPath : IConfigMapPath
        {
            private readonly HttpSimulator requestSimulation;
            public ConfigMapPath(HttpSimulator simulation)
            {
                requestSimulation = simulation;
            }

            public string GetMachineConfigFilename()
            {
                throw new NotImplementedException();
            }

            public string GetRootWebConfigFilename()
            {
                throw new NotImplementedException();
            }

            public void GetPathConfigFilename(string siteID, string path, out string directory, out string baseName)
            {
                throw new NotImplementedException();
            }

            public void GetDefaultSiteNameAndID(out string siteName, out string siteID)
            {
                throw new NotImplementedException();
            }

            public void ResolveSiteArgument(string siteArgument, out string siteName, out string siteID)
            {
                throw new NotImplementedException();
            }

            public string MapPath(string siteID, string path)
            {
                string page = StripPrecedingSlashes(RightAfter(path, requestSimulation.ApplicationPath));
                return Path.Combine(requestSimulation.PhysicalApplicationPath, page.Replace("/", @"\"));
            }

            public string GetAppPathForPath(string siteID, string path)
            {
                return requestSimulation.ApplicationPath;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current = null;
            }
        }
    }
}