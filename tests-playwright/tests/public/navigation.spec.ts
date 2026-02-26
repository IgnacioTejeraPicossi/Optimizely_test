import { test, expect } from "@playwright/test";
import { stabilizePage } from "../../test-utils/stabilize";

test.describe("Public navigation", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("/");
    await stabilizePage(page);
  });

  test("nav has Home and Search links", async ({ page }) => {
    const nav = page.locator('[data-testid="site-nav"]');
    await expect(nav.locator('[data-testid="nav-home"]')).toBeVisible();
    await expect(nav.locator('[data-testid="nav-search"]')).toBeVisible();
  });

  test("Home link goes to start page", async ({ page }) => {
    await page.locator('[data-testid="nav-home"]').click();
    await expect(page).toHaveURL(/\//);
    await expect(page.locator('[data-testid="start-page"]')).toBeVisible();
  });

  test("Search link goes to search page", async ({ page }) => {
    await page.locator('[data-testid="nav-search"]').click();
    await expect(page).toHaveURL(/\/search/);
    await expect(page.locator('[data-testid="search-page"]')).toBeVisible();
  });

  test("personal pages links exist and navigate", async ({ page }) => {
    const nav = page.locator('[data-testid="site-nav"]');

    const links = [
      { name: "About Me", testId: "about-me-page" },
      { name: "AI tools", testId: "ai-tools-page" },
      { name: "My Hobbies", testId: "hobbies-page" },
      { name: "Contact Me", testId: "contact-page" },
    ];

    for (const { name, testId } of links) {
      const link = nav.getByRole("link", { name });
      await expect(link).toBeVisible();
      await link.click();
      await expect(page.locator(`[data-testid="${testId}"]`)).toBeVisible();
      await page.goto("/");
      await stabilizePage(page);
    }
  });
});
