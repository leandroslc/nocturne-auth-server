import { Consent } from 'cookieconsent';

declare global {
  interface Window {
    cookieconsent: Consent;
  }
}

const MetaNamePreix = 'cookie-connsent';

const getMetaSelector = (name: string) => `meta[name="${name}"]`;

const getMetaValue = (name: string) => {
  const meta = document.head.querySelector(
    getMetaSelector(`${MetaNamePreix}-${name}`),
  );

  return meta ? meta.getAttribute('content') : null;
};

function initialize() {
  const message = getMetaValue('message');
  const dismiss = getMetaValue('dismiss');

  if (!message) {
    return;
  }

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
      message,
      dismiss: dismiss || 'OK',
    },
  });
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', initialize);
} else {
  initialize();
}
