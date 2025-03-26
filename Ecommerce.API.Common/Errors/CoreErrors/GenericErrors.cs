using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API.Common.Errors.CoreErrors
{
    public class GenericErrors
    {
        public static readonly CustomError IntegrationError = new CustomError(HttpStatusCode.InternalServerError, "something_went_wrong", "Something went wrong");
        public static readonly CustomError ThirdPartyFailure = new CustomError(HttpStatusCode.InternalServerError, "third_party_failure", "Third Party Failure");
        public static CustomError NotFound(string entityName) => new CustomError(HttpStatusCode.NotFound, "not_found", $"{entityName} Not found");
        public static CustomError NotFound(string entityName, string id) => new CustomError(HttpStatusCode.NotFound, "not_found", $"{entityName} Not found with id {id}");

        public static CustomError RequiredParameter(string parameterName) => new CustomError(HttpStatusCode.BadRequest, "parameter_required", $"{parameterName} is required");
        public static CustomError AlreadyExists(string entityName, string system) => new CustomError(HttpStatusCode.BadRequest, "already_exists", $"{entityName} already exists in {system}");


    }
}
