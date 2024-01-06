using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Helpers
{
    public class ValidationHelper
    {
        //La validación automática contra atributos del modelo sólo se aplica de forma automática para 
        //los parámetros de cuerpo de la solicitud. ValidateRouteParams lo realiza con parámetros de la ruta.
        //Forma de uso: pasar el modelo con las propiedades a validar asignadas.
        public static ActionResult ValidateRouteParams(ControllerBase controller, object instance, string modelKey="")
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(instance, serviceProvider: null, items: null);

            if (!Validator.TryValidateObject(instance, validationContext, validationResults, validateAllProperties: true))
            {
                foreach (var validationResult in validationResults)
                {
                    var key = string.IsNullOrEmpty(modelKey) ? validationResult.MemberNames.FirstOrDefault() ?? "" : modelKey;
                    controller.ModelState.AddModelError(key, validationResult.ErrorMessage);
                }

                return controller.BadRequest(controller.ModelState);
            }
            return null;
        }
    }
}
