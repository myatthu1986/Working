﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.IdentityModel.Tokens;
using SG50.Model.Models.Entities;
using Microsoft.Owin.Security.DataHandler.Encoder;
using SG50.Base.Util;

namespace SG50.Common
{   
    public class CustomizedAuthorizationAttribute : AuthorizationFilterAttribute
    {   
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

                if (actionContext.Request.Headers.Authorization.Parameter == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return Task.FromResult<object>(null);
                }

                string EncodedJwtToken = actionContext.Request.Headers.Authorization.Parameter;
                var _JwtSecurityToken = new JwtSecurityToken(EncodedJwtToken);
                string AudienceId = _JwtSecurityToken.Audiences.First();
                string SecurityKey = string.Empty;

                using(SG50DBEntities _SG50DBEntities = new SG50DBEntities())
                {
                    tbl_AppActiveUser _tbl_AppActiveUser = _SG50DBEntities.tbl_AppActiveUser.Where(x => x.Id.Equals(new Guid(AudienceId))).FirstOrDefault();
                    if (_tbl_AppActiveUser == null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return Task.FromResult<object>(null);
                    }
                    SecurityKey = _tbl_AppActiveUser.JwtHMACKey;

                    var _JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var _JwtSecurityTokenHandler_JWTToken = _JwtSecurityTokenHandler.ReadToken(EncodedJwtToken);

                    SecurityToken _SecurityToken = null;
                    _JwtSecurityTokenHandler.ValidateToken(
                        EncodedJwtToken,
                        new TokenValidationParameters()
                        {
                            IssuerSigningKey = new InMemorySymmetricSecurityKey(TextEncodings.Base64Url.Decode(SecurityKey)),
                            ValidAudience = _tbl_AppActiveUser.Id.ToString(),                                                     
                            ValidIssuer = AppConfiger.UrlTokenIssuer,
                            ValidateLifetime = false,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true
                        }, out _SecurityToken);

                    if (_SecurityToken == null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return Task.FromResult<object>(null);
                    }
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);
        }
    }
}
