/// <reference types="cypress" />

describe('Home page (StartPage)', () => {
  beforeEach(() => {
    cy.visit('/');
  });

  it('returns 200 and loads the page', () => {
    cy.url().should('include', '/');
    cy.get('body').should('be.visible');
  });

  it('renders start page structure', () => {
    cy.getByTestId('start-page').should('exist');
  });

  it('renders main heading', () => {
    cy.getByTestId('main-heading').should('exist').and('be.visible');
  });

  it('renders site header and footer', () => {
    cy.getByTestId('site-header').should('exist');
    cy.getByTestId('site-footer').should('exist');
  });

  it('renders navigation', () => {
    cy.getByTestId('site-nav').should('exist');
    cy.getByTestId('nav-home').should('exist');
    cy.getByTestId('nav-search').should('exist');
  });

  it('contains expected text', () => {
    cy.contains('Optimizely CMS is running').should('be.visible');
  });

  it('renders main content area', () => {
    cy.getByTestId('main-content').should('exist');
  });
});
