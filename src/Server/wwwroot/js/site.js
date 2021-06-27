
(function () {

  const validationOptions = {
    errorClass: 'text-danger',
    ignore: '.tagify,.tagify>*',
  };

  $.validator.setDefaults(validationOptions);
  $.validator.unobtrusive.options = $.validator.defaults;

  window.Validator = {
    attach: function(form) {
      $.validator.unobtrusive.parse(form);
    },

    isValid: function(form) {
      return $(form).valid();
    },
  };

})();

(function () {
  window.Loading.setIsValidHandler(function (form) {
    if (form.checkValidity && form.checkValidity() === false) {
      return false;
    }

    const $form = $(form);

    if ($form.valid && $form.valid() === false) {
      return false;
    }

    return true;
  });
})();

(function () {
  const IsPasswordAttribute = 'is-password';
  const TypeAttribute = 'type';

  function togglePassword(targetElement, input) {
    const isPassword = input.type === 'password';
    const isPasswordElements = targetElement.querySelectorAll('[' + IsPasswordAttribute + ']');

    isPasswordElements.forEach(function(e) {
      e.getAttribute(IsPasswordAttribute) === 'true'
        ? e.style.display = isPassword ? 'none' : ''
        : e.style.display = isPassword ? '' : 'none';
    });

    isPassword
      ? input.setAttribute(TypeAttribute, 'text')
      : input.setAttribute(TypeAttribute, 'password');
  }

  window.ElementHelper = {
    addTogglePasswordEvent(type, targetElement, input) {
      targetElement.addEventListener(type, function (event) {
        event.preventDefault();

        togglePassword(targetElement, input);
      });
    }
  };
})();
