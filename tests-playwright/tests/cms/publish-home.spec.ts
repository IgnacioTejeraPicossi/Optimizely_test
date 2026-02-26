import { test, expect } from "@playwright/test";

/**
 * CMS: Publish Home page.
 * Uses storageState. After publish, public site should show updated content.
 */
test.describe("CMS - Publish Home", () => {
  test.use({ storageState: "storage/auth.json" });

  test.skip("Publish Home and verify public site", async ({ page }) => {
    await page.goto("/episerver/cms");

    // TODO: Record with codegen: open Home -> Publish -> then verify public /
    await expect(page.getByText(/CMS/i)).toBeVisible();
  });
});
