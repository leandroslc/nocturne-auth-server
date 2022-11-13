import { ValidatorProvider } from '@/src/base/providers/types';

const validationOptions = {
  errorClass: 'text-danger',
  ignore: '.tagify,.tagify>*',
};

$.validator.setDefaults(validationOptions);
$.validator.unobtrusive.options = $.validator.defaults;

const provider: ValidatorProvider = {
  attach: (form) => {
    $.validator.unobtrusive.parse(form);
  },

  isValid: (form) => {
    return $(form).valid();
  },
};

export default provider;
