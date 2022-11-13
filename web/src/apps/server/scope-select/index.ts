const ScopeAttribute = 'scope';
const ScopeSelector = '[name="scope"]';

window.scopeSelect = (options) => {
  const scopesContainer = window.ElementHelper.getRequiredElementById(
    options.scopesContainerId,
  );
  const scopeInput =
    window.ElementHelper.queryRequiredElementBySelector<HTMLInputElement>(
      ScopeSelector,
    );

  const addScope = (newScope: string) => {
    scopeInput.value = `${scopeInput.value} ${newScope}`;
  };

  const removeScope = (scopeToRemove: string) => {
    scopeInput.value = scopeInput.value.replace(
      new RegExp(`(\\s+${scopeToRemove}$)|(${scopeToRemove}\\s+)`),
      '',
    );
  };

  scopesContainer.addEventListener('change', (event) => {
    const targetElement = event.target as Element;
    const element = targetElement.closest<HTMLInputElement>(
      `[${ScopeAttribute}]`,
    );

    if (!element) {
      return;
    }

    const selectedScope = element.getAttribute(ScopeAttribute)!;

    if (element.checked) {
      addScope(selectedScope);
    } else {
      removeScope(selectedScope);
    }
  });
};
