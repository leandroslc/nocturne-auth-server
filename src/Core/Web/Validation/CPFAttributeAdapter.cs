using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Shared.Validation;

namespace Nocturne.Auth.Core.Web.Validation
{
    public class CPFAttributeAdapter : AttributeAdapterBase<CPFAttribute>
    {
        public CPFAttributeAdapter(
            CPFAttribute attribute,
            IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            Attribute.ErrorMessage = stringLocalizer[Attribute.ErrorMessage];
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-cpf", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return Attribute.FormatErrorMessage(
                validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
