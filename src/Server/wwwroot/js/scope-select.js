
(function () {
  const ScopeAttribute = 'scope';
  const ScopeSelector = '[name="scope"]';

  window.scopeSelect = function(options) {
    const scopesContainer = document.getElementById(options.scopesContainerId);
    const scopeInput = document.querySelector(ScopeSelector);

    function addScope(newScope) {
      scopeInput.value = scopeInput.value + ' ' + newScope;
    }

    function removeScope(scopeToRemove) {
      scopeInput.value = scopeInput.value.replace(
        new RegExp('(\\s+'+ scopeToRemove + '\$)|(' + scopeToRemove + '\\s+)'), '');
    }

    scopesContainer.addEventListener('change', function (event) {
        const element = event.target.closest('[' + ScopeAttribute + ']');

        if (!element) {
            return;
        }

        const selectedScope = element.getAttribute(ScopeAttribute);

        if (element.checked) {
          addScope(selectedScope);
        } else {
          removeScope(selectedScope);
        }
    });
  }
})();
