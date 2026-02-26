import { test, expect } from "@playwright/test";
import { stabilizePage } from "../../test-utils/stabilize";

test.describe("Public Home", () => {
  test("loads and shows heading", async ({ page }) => {
    await page.goto("/");
    await stabilizePage(page);

    await expect(page.getByRole("heading", { name: /Hello Optimizely/i })).toBeVisible();
    await expect(page.getByText(/Optimizely CMS is running/i)).toBeVisible();
  });

  test("has start page structure and main content", async ({ page }) => {
    await page.goto("/");
    await stabilizePage(page);

    await expect(page.locator('[data-testid="start-page"]')).toBeVisible();
    await expect(page.locator('[data-testid="main-heading"]')).toBeVisible();
    await expect(page.locator('[data-testid="main-content"]')).toBeVisible();
  });

  test("has site header and footer", async ({ page }) => {
    await page.goto("/");
    await stabilizePage(page);

    await expect(page.locator('[data-testid="site-header"]')).toBeVisible();
    await expect(page.locator('[data-testid="site-footer"]')).toBeVisible();
  });
});
