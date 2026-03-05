import * as fs from "fs";
import * as path from "path";
import { test, expect } from "@playwright/test";

const authPath = path.join(process.cwd(), "storage", "auth.json");
const hasAuth = () => fs.existsSync(authPath);

/**
 * CMS: Publish Home page.
 * Uses storageState. After publish, public site should show updated content.
 */
test.describe("CMS - Publish Home", () => {
  test.use(hasAuth() ? { storageState: authPath } : {});

  test.skip("Publish Home and verify public site", async ({ page }) => {
    await page.goto("/episerver/cms");

    // TODO: Record with codegen: open Home -> Publish -> then verify public /
    await expect(page.getByText(/CMS/i)).toBeVisible();
  });
});
