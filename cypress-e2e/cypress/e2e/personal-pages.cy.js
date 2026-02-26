/// <reference types="cypress" />

const personalPages = [
  { path: '/about-me/', testId: 'about-me-page', title: 'About Me', content: 'IGNACIO TEJERA' },
  { path: '/ai-tools/', testId: 'ai-tools-page', title: 'AI Tools', content: 'AI I Enjoy using' },
  { path: '/my-hobbies/', testId: 'hobbies-page', title: 'My Hobbies', content: 'Things I Enjoy Doing' },
  { path: '/contact-me/', testId: 'contact-page', title: 'Contact Me', content: 'Get in touch' },
];

describe('Personal pages', () => {
  personalPages.forEach(({ path, testId, title, content }) => {
    describe(`${title} page`, () => {
      beforeEach(() => {
        cy.visit(path);
      });

      it('loads the page and shows correct structure', () => {
        const slug = path.replace(/^\//, '').replace(/\/$/, '');
        cy.url().should('include', slug);
        cy.getByTestId(testId).should('exist');
      });

      it('renders personal layout with header and sidebar', () => {
        cy.getByTestId('site-header').should('exist');
        cy.getByTestId('site-nav').should('exist');
        cy.getByTestId('personal-layout').should('exist');
        cy.getByTestId('personal-sidebar').should('exist');
        cy.getByTestId('main-content').should('exist');
      });

      it('shows page title and expected content', () => {
        cy.contains(title).should('be.visible');
        cy.contains(content).should('be.visible');
      });

      it('renders site footer', () => {
        cy.getByTestId('site-footer').should('exist');
      });
    });
  });

  describe('Navigation from home', () => {
    beforeEach(() => {
      cy.visit('/');
    });

    it('home page has links to all personal pages', () => {
      cy.getByTestId('site-nav').within(() => {
        cy.contains('a', 'About Me').should('exist').and('have.attr', 'href').and('include', 'about-me');
        cy.contains('a', 'AI tools').should('exist').and('have.attr', 'href').and('include', 'ai-tools');
        cy.contains('a', 'My Hobbies').should('exist').and('have.attr', 'href').and('include', 'my-hobbies');
        cy.contains('a', 'Contact Me').should('exist').and('have.attr', 'href').and('include', 'contact-me');
      });
    });
  });
});
