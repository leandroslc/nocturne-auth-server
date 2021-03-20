
(function () {

  const validationOptions = {
    errorClass: 'text-danger',
    ignore: '.tagify,.tagify>*',
  };

  $.validator.setDefaults(validationOptions);
  $.validator.unobtrusive.options = validationOptions;

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
