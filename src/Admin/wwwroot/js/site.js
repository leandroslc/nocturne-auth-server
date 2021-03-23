
(function () {

  const validationOptions = {
    errorClass: 'text-danger',
    ignore: '.tagify,.tagify>*',
  };

  $.validator.setDefaults(validationOptions);
  $.validator.unobtrusive.options = validationOptions;

})();

(function() {
  const IsPasswordAttribute = 'is-password';

  function togglePassword(element, button) {
    const isPassword = element.type === 'password';
    const isPasswordElements = button.querySelectorAll('[' + IsPasswordAttribute + ']');

    isPasswordElements.forEach(function(e) {
      e.getAttribute(IsPasswordAttribute) === 'true'
        ? e.style.display = isPassword ? 'none' : ''
        : e.style.display = isPassword ? '' : 'none';
    });

    isPassword
      ? element.setAttribute('type', 'text')
      : element.setAttribute('type', 'password');
  }

  window.ElementHelper = {
    addTogglePasswordEvent: function(element, button) {
      button.addEventListener('click', function() {
        togglePassword(element, button);
      });
    },

    copyText: function(element) {
      const isPassword = element.type === 'password';

      element.select();
      element.setSelectionRange(0, 99999);

      isPassword && element.setAttribute('type', 'text');

      document.execCommand("copy");

      isPassword && element.setAttribute('type', 'password');
    },

    getRequiredElementById: function(id) {
      const element = document.getElementById(id);

      if (element) {
        return element;
      }

      throw new Error('Element "' + id + '" not found');
    },
  };

})();

(function () {

  function TagifyElement(element, optionsBuilder) {
    const options = {
      delimiters: ',| ',
      keepInvalidTags: false,
      autoComplete: {
        enabled: false,
      },
      editTags: {
        clicks: 1,
        keepInvalid: false,
      },
      originalInputValueFormat: function(values) {
        return values.map(item => item.value).join(' ');
      },
    };

    if (optionsBuilder) {
      optionsBuilder(options);
    }

    const tagify = new Tagify(element, options);

    this.disable = function() {
      tagify.setReadonly(true);
      element.setAttribute('disabled', '');
    }

    this.clean = function() {
      tagify.removeAllTags();
      element.value = '';
    }

    this.disableAndClean = function() {
      this.disable(tagify, element);
      this.clean(tagify, element);
    }

    this.enable = function() {
      tagify.setReadonly(false);
      element.removeAttribute('disabled');
    }
  }

  window.TagifyElement = TagifyElement;

})();
