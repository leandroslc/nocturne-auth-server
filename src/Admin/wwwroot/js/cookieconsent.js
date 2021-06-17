
window.cookieconsent.show = function(options) {
  const message = options.message;
  const dismiss = options.dismiss;

  window.cookieconsent.initialise({
    palette: {
      popup: {
        background: '#252e39',
      },
      button: {
        background: '#14a7d0',
      },
    },
    theme: 'classic',
    position: 'bottom-right',
    showLink: false,
    content: {
      message: message,
      dismiss: dismiss,
    },
  });
}
