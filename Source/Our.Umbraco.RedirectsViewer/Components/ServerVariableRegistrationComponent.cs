﻿using Our.Umbraco.RedirectsViewer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.JavaScript;

namespace Our.Umbraco.RedirectsViewer.Components
{
    internal class ServerVariableRegistrationComponent : IComponent
    {
        public void Initialize()
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        public void Terminate()
        {
            ServerVariablesParser.Parsing -= ServerVariablesParser_Parsing;
        }

        private void ServerVariablesParser_Parsing(object sender, System.Collections.Generic.Dictionary<string, object> e)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            SetUpDictionaryForAngularPropertyEditor(e);
        }

        private static void SetUpDictionaryForAngularPropertyEditor(Dictionary<string, object> e)
        {
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

            var urlDictionairy = new Dictionary<string, object>
            {
                {
                    "UserGroupApi",
                    urlHelper.GetUmbracoApiServiceBaseUrl<UserGroupsApiController>(c => c.GetUserGroups())
                },
                {
                    "RedirectsApi", urlHelper.GetUmbracoApiServiceBaseUrl<RedirectsApiController>(c =>
                        c.GetRedirectsForContent(Guid.Empty, string.Empty))
                }
            };


            if (!e.Keys.Contains("Our.Umbraco.RedirectsViewer"))
            {
                e.Add("Our.Umbraco.RedirectsViewer", urlDictionairy);
            }
        }
    }
}
