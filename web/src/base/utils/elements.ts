const DisabledAttribute = 'disabled';

type ElementCollection = NodeListOf<Element> | Element[];

export function setDisabled(element: Element) {
  element.setAttribute(DisabledAttribute, DisabledAttribute);
}

export function setDisabledAll(elements: ElementCollection) {
  elements.forEach((element: Element) => setDisabled(element));
}

export function setEnabled(element: Element) {
  element.removeAttribute(DisabledAttribute);
}

export function setEnabledAll(elements: ElementCollection) {
  elements.forEach((element: Element) => setEnabled(element));
}

export function show(element: HTMLElement) {
  // eslint-disable-next-line no-param-reassign
  element.style.display = '';
}

export function hide(element: HTMLElement) {
  // eslint-disable-next-line no-param-reassign
  element.style.display = 'none';
}

export function tryGetElementById(id: string) {
  return document.getElementById(id) || undefined;
}
