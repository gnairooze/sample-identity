using IdentitySample.SharedModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SampleIdentity.Api.IdentityBusiness;
using SampleIdentity.Api.IdentityModels;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net.Http;

namespace SampleIdentity.Api.IdentityControllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }

        // Existing code...

        [AllowAnonymous]

        [Route("Register")]

        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState);

            }



            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };



            IdentityResult result = await UserManager.CreateAsync(user, model.Password);



            if (!result.Succeeded)

            {

                return GetErrorResult(result);

            }



            return Ok();

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}
