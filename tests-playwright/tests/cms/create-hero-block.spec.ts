import { test, expect } from "@playwright/test";

/**
 * CMS: Create Hero Block.
 * Uses storageState from login.setup (run cms:login first).
 *
 * Flow: open CMS -> navigate to Home -> Hero Area -> Create new Hero Block -> fill Heading/Text -> Create/Publish.
 * Use codegen to capture real selectors: npx playwright codegen https://localhost:5000/episerver/cms
 */
test.describe("CMS - Create Hero Block", () => {
  test.use({ storageState: "storage/auth.json" });

  test("CMS loads after login", async ({ page }) => {
    await page.goto("/episerver/cms");

    await expect(page.getByText(/CMS|Optimizely|Edit/i)).toBeVisible({ timeout: 10_000 });
  });

  test.skip("Create Hero Block and fill Heading/Text", async ({ page }) => {
    await page.goto("/episerver/cms");

    // TODO: Use Playwright codegen to record:
    // 1) Open Home page in tree
    // 2) Hero Area -> Select content -> Create new -> Hero Block
    // 3) Fill Heading, Text -> Create / Publish
    await expect(page.getByText(/CMS/i)).toBeVisible();
  });
});
