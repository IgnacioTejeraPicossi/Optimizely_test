/// <reference types="cypress" />

describe('404 Not Found', () => {
  it('shows not found page for unknown route', () => {
    cy.visit('/non-existent-page-xyz', { failOnStatusCode: false });
    cy.getByTestId('not-found-page').should('exist');
    cy.getByTestId('not-found-heading').should('exist');
    cy.getByTestId('not-found-home-link').should('exist').and('have.attr', 'href').and('include', '/');
  });

  it('home link returns to start page', () => {
    cy.visit('/non-existent-page-xyz', { failOnStatusCode: false });
    cy.getByTestId('not-found-home-link').click();
    cy.url().should('match', /\/(\?.*)?$/);
    cy.getByTestId('start-page').should('exist');
  });
});
