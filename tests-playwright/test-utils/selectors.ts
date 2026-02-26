/**
 * Centralized selectors for Optimizely Demo CMS (public site).
 * Prefer data-testid where available; fallback to role/label.
 */
export const publicSelectors = {
  startPage: '[data-testid="start-page"]',
  mainHeading: '[data-testid="main-heading"]',
  mainContent: '[data-testid="main-content"]',
  heroArea: '[data-testid="hero-area"]',
  heroBlock: '[data-testid="hero-block"]',
  siteHeader: '[data-testid="site-header"]',
  siteNav: '[data-testid="site-nav"]',
  siteFooter: '[data-testid="site-footer"]',
  navHome: '[data-testid="nav-home"]',
  navSearch: '[data-testid="nav-search"]',
  searchPage: '[data-testid="search-page"]',
  searchForm: '[data-testid="search-form"]',
  searchInput: '[data-testid="search-input"]',
  searchSubmit: '[data-testid="search-submit"]',
  notFoundPage: '[data-testid="not-found-page"]',
  aboutMePage: '[data-testid="about-me-page"]',
  aiToolsPage: '[data-testid="ai-tools-page"]',
  hobbiesPage: '[data-testid="hobbies-page"]',
  contactPage: '[data-testid="contact-page"]',
} as const;
