﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JIC.Utilities.MvcHelpers
{
    public class CJavaScriptResult : JavaScriptResult
    {
        private List<string> SuccessMessages = new List<string>();
        private List<string> ErrorMessages = new List<string>();
        private Exception exception;
        public CJavaScriptResult(string URL)
        {
            Script = "window.location = '" + URL + "'";
        }
        public CJavaScriptResult WithSuccessMessages(params string[] messages)
        {
            SuccessMessages.AddRange(messages);
            return this;
        }
        public CJavaScriptResult WithErrorMessages(params string[] messages)
        {
            ErrorMessages.AddRange(messages);
            return this;
        }
        public CJavaScriptResult WithException(Exception exception)
        {
            this.exception = exception;
            return this;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (SuccessMessages.Count > 0)
                context.Controller.TempData["SuccessMessages"] = SuccessMessages.ToArray();
            if(ErrorMessages.Count > 0)
                context.Controller.TempData["ErrorMessages"] = ErrorMessages.ToArray();
            if(exception != null)
                context.Controller.TempData["ExceptionMessage"] = exception;
            base.ExecuteResult(context);
        }
    }
}