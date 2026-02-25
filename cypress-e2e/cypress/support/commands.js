// Custom Cypress commands (optional)
// Usage: cy.getByTestId('start-page')
Cypress.Commands.add('getByTestId', (testId) => {
  return cy.get(`[data-testid="${testId}"]`);
});
