/// <reference types="cypress" />

describe('Health endpoints (smoke)', () => {
  it('GET /health returns JSON with status', () => {
    cy.request({
      url: '/health',
      failOnStatusCode: false,
    }).then((res) => {
      expect(res.status).to.be.oneOf([200, 503]);
      expect(res.headers['content-type']).to.include('application/json');
      expect(res.body).to.have.property('status');
    });
  });

  it('GET /health/ready returns JSON', () => {
    cy.request({
      url: '/health/ready',
      failOnStatusCode: false,
    }).then((res) => {
      expect(res.status).to.be.oneOf([200, 503]);
      expect(res.headers['content-type']).to.include('application/json');
    });
  });
});
