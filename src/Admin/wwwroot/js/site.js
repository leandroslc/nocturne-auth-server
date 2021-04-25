
// Polyfills
(function () {

  // https://developer.mozilla.org/en-US/docs/Web/API/Element/matches
  if (!Element.prototype.matches) {
    Element.prototype.matches =
      Element.prototype.msMatchesSelector ||
      Element.prototype.webkitMatchesSelector;
  }

  // https://developer.mozilla.org/en-US/docs/Web/API/Element/closest
  if (!Element.prototype.closest) {
    Element.prototype.closest = function(s) {
      var el = this;

      do {
        if (Element.prototype.matches.call(el, s)) return el;
        el = el.parentElement || el.parentNode;
      } while (el !== null && el.nodeType === 1);
      return null;
    };
  }
})();

(function () {

  const validationOptions = {
    errorClass: 'text-danger',
    ignore: '.tagify,.tagify>*',
  };

  $.validator.setDefaults(validationOptions);
  $.validator.unobtrusive.options = validationOptions;

  window.Validator = {
    attach: function(form) {
      $.validator.unobtrusive.parse(form);
    },

    isValid: function(form) {
      return $(form).valid();
    },
  };

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

    // Store and restore active element functions are workarounds to prevent
    // tagfy from stealing focus from other elements.
    var currentActiveElement;

    function storeActiveElement() {
      currentActiveElement = document.activeElement;
    }

    function restoreActiveElement() {
      currentActiveElement && currentActiveElement.focus();
    }

    this.disable = function() {
      storeActiveElement();

      tagify.setReadonly(true);
      element.setAttribute('disabled', '');

      restoreActiveElement();
    }

    this.clean = function() {
      storeActiveElement();

      tagify.removeAllTags();
      element.value = '';

      restoreActiveElement();
    }

    this.disableAndClean = function() {
      this.disable(tagify, element);
      this.clean(tagify, element);
    }

    this.enable = function() {
      storeActiveElement();

      tagify.setReadonly(false);
      element.removeAttribute('disabled');

      restoreActiveElement();
    }
  }

  window.TagifyElement = TagifyElement;

})();

(function () {
  function Modal(id) {
    const element = document.getElementById(id);

    if (!element) {
      throw new Error('Element no found');
    }

    const content = element.querySelector('.modal-content');
    const $modal = $(element);

    $modal.on('shown.bs.modal', function () {
      $modal.find('[autofocus]').focus();
    });

    $modal.on('hidden.bs.modal', function () {
      $modal.find('input, select, textarea').val('');
    });

    this.show = function() {
      $modal.modal('show');
    }

    this.close = function() {
      $modal.modal('hide');
    }

    this.setContent = function(html) {
      content.innerHTML = html;
    }

    this.querySelector = function(selector) {
      return content.querySelector(selector);
    }
  }

  window.Modal = Modal;
})();
