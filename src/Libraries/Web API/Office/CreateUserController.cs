// ReSharper disable All
using MixERP.Net.Framework;
using MixERP.Net.Entities.Office;
using MixERP.Net.Schemas.Office.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MixERP.Net.ApplicationState.Cache;
using MixERP.Net.Common.Extensions;
using PetaPoco;
using MixERP.Net.EntityParser;
namespace MixERP.Net.Api.Office
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function CreateUser.
    /// </summary>
    [RoutePrefix("api/v1.5/office/procedures/create-user")]
    public class CreateUserController : ApiController
    {
        /// <summary>
        /// Login id of application user accessing this API.
        /// </summary>
        public long _LoginId { get; set; }

        /// <summary>
        /// User id of application user accessing this API.
        /// </summary>
        public int _UserId { get; set; }

        /// <summary>
        /// Currently logged in office id of application user accessing this API.
        /// </summary>
        public int _OfficeId { get; set; }

        /// <summary>
        /// The name of the database where queries are being executed on.
        /// </summary>
        public string _Catalog { get; set; }

        private CreateUserProcedure procedure;
        public class Annotation
        {
            public int RoleId { get; set; }
            public int DepartmentId { get; set; }
            public int OfficeId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public bool Elevated { get; set; }
        }

        public CreateUserController()
        {
            this._LoginId = AppUsers.GetCurrent().View.LoginId.ToLong();
            this._UserId = AppUsers.GetCurrent().View.UserId.ToInt();
            this._OfficeId = AppUsers.GetCurrent().View.OfficeId.ToInt();
            this._Catalog = AppUsers.GetCurrentUserDB();
            this.procedure = new CreateUserProcedure
            {
                _Catalog = this._Catalog,
                _LoginId = this._LoginId,
                _UserId = this._UserId
            };
        }
        /// <summary>
        ///     Creates meta information of "create user" annotation.
        /// </summary>
        /// <returns>Returns the "create user" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/office/procedures/create-user/annotation")]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                                {
                                        new EntityColumn { ColumnName = "_role_id",  PropertyName = "RoleId",  DataType = "int",  DbDataType = "integer",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 },
                                        new EntityColumn { ColumnName = "_department_id",  PropertyName = "DepartmentId",  DataType = "int",  DbDataType = "integer",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 },
                                        new EntityColumn { ColumnName = "_office_id",  PropertyName = "OfficeId",  DataType = "int",  DbDataType = "integer",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 },
                                        new EntityColumn { ColumnName = "_user_name",  PropertyName = "UserName",  DataType = "string",  DbDataType = "text",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 },
                                        new EntityColumn { ColumnName = "_password",  PropertyName = "Password",  DataType = "string",  DbDataType = "text",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 },
                                        new EntityColumn { ColumnName = "_full_name",  PropertyName = "FullName",  DataType = "string",  DbDataType = "text",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 },
                                        new EntityColumn { ColumnName = "_elevated",  PropertyName = "Elevated",  DataType = "boolean DEFAULT false",  DbDataType = "boolean DEFAULT false",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 }
                                }
            };
        }

        /// <summary>
        ///     Creates meta information of "create user" entity.
        /// </summary>
        /// <returns>Returns the "create user" meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("meta")]
        [Route("~/api/office/procedures/create-user/meta")]
        public EntityView GetEntityView()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                }
            };
        }

        [AcceptVerbs("POST")]
        [Route("execute")]
        [Route("~/api/office/procedures/create-user/execute")]
        public void Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.procedure.RoleId = annotation.RoleId;
                this.procedure.DepartmentId = annotation.DepartmentId;
                this.procedure.OfficeId = annotation.OfficeId;
                this.procedure.UserName = annotation.UserName;
                this.procedure.Password = annotation.Password;
                this.procedure.FullName = annotation.FullName;
                this.procedure.Elevated = annotation.Elevated;


                this.procedure.Execute();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (MixERPException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    }
}