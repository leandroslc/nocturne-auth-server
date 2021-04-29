(function () {
  window.SearchRoles = function(options) {
    const form = options.searchForm;
    const searchUrl = options.searchUrl;
    const results = options.results;

    const self = this;
    const formLoading = Loading.create(form);

    form.addEventListener('submit', function (event) {
      event.preventDefault();

      self.search();
    });

    this.search = function() {
      formLoading.start();

      const data = $(form).serialize();

      $.get(searchUrl, data, function (response) {
        results.innerHTML = response;
        formLoading.stop();
      });
    }
  };
})();

(function () {
  const PermissionActionAttribute = 'data-role-action';

  window.manageRoles = function(options) {
    const searchPermissions = new SearchPermissions(options);
    const results = options.results;
    const modalId = options.modalId;

    const modal = new Modal(modalId);

    function createModalFormSubmitEvent(form, url) {
      const formLoading = Loading.create(form);

      return function(event) {
        event.preventDefault();

        if (!Validator.isValid(form)) {
          return;
        }

        formLoading.start();

        const data = $(form).serialize();

        $.post(url, data, function () {
          searchPermissions.search();
          modal.close();
        }).fail(function (response) {
          if (response.status === 400) {
            setupModal(response.responseText, url);
          }
        });
      };
    }

    function setupModalForm(url) {
      const form = modal.querySelector('form');

      if (!form) {
        return;
      }

      Validator.attach(form);

      form.addEventListener('submit', createModalFormSubmitEvent(form, url));
    }

    function setupModal(content, url) {
      modal.setContent(content);
      setupModalForm(url);
    }

    function show(element) {
      const url = element.getAttribute(PermissionActionAttribute);

      $.get(url, function (response) {
        setupModal(response, url);
        modal.show();
      });
    }

    results.addEventListener('click', function (event) {
      const element = event.target.closest('[' + PermissionActionAttribute + ']');

      if (!element) {
        return;
      }

      show(element);
    });

    searchPermissions.search();
  }
})();
