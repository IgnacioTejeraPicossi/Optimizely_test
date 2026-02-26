import { test, expect } from "@playwright/test";
import { stabilizePage } from "../../test-utils/stabilize";

test.describe("Public Home - visual regression", () => {
  test("Home - full page snapshot", async ({ page }) => {
    await page.goto("/");
    await stabilizePage(page);

    await expect(page.locator('[data-testid="start-page"]')).toBeVisible();
    await expect(page).toHaveScreenshot("home-full.png", { fullPage: true });
  });
});
