import { test, expect } from "@playwright/test";
import { stabilizePage } from "../../test-utils/stabilize";

test.describe("Hero block on public Home", () => {
  test("hero area is present when page loads", async ({ page }) => {
    await page.goto("/");
    await stabilizePage(page);

    const startPage = page.locator('[data-testid="start-page"]');
    await expect(startPage).toBeVisible();
    // Hero area may be empty; if blocks exist, hero-block is rendered
    const heroArea = page.locator('[data-testid="hero-area"]');
    const heroBlock = page.locator('[data-testid="hero-block"]');
    if (await heroBlock.count() > 0) {
      await expect(heroBlock.first()).toBeVisible();
    }
    // At least main heading is always visible
    await expect(page.locator('[data-testid="main-heading"]')).toBeVisible();
  });

  test("when Hero block exists, heading and content are visible", async ({ page }) => {
    await page.goto("/");
    await stabilizePage(page);

    const heroBlock = page.locator('[data-testid="hero-block"]');
    const count = await heroBlock.count();
    if (count === 0) {
      test.skip();
      return;
    }
    await expect(heroBlock.first()).toBeVisible();
    const firstBlock = heroBlock.first();
    await expect(firstBlock.locator("h1")).toBeVisible();
  });
});
