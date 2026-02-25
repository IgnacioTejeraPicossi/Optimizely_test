/// <reference types="cypress" />

describe('Search page', () => {
  beforeEach(() => {
    cy.visit('/search');
  });

  it('loads the search page', () => {
    cy.url().should('include', '/search');
    cy.getByTestId('search-page').should('exist');
  });

  it('renders search form', () => {
    cy.getByTestId('search-form').should('exist');
    cy.getByTestId('search-input').should('exist');
    cy.getByTestId('search-submit').should('exist');
  });

  it('shows search heading', () => {
    cy.getByTestId('search-heading').should('exist').and('contain', 'Search');
  });

  it('can submit a search (empty query)', () => {
    cy.getByTestId('search-submit').click();
    cy.url().should('include', '/search');
  });

  it('can type in search input', () => {
    cy.getByTestId('search-input').type('test');
    cy.getByTestId('search-input').should('have.value', 'test');
  });
});
