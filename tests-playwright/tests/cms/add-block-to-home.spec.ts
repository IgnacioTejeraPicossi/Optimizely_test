import * as fs from "fs";
import * as path from "path";
import { test, expect } from "@playwright/test";

const authPath = path.join(process.cwd(), "storage", "auth.json");
const hasAuth = () => fs.existsSync(authPath);

/**
 * CMS: Add created Hero Block to Home Hero Area.
 * Depends on create-hero-block (or existing block). Uses storageState.
 *
 * Flow: open Home in CMS -> Hero Area -> add block (existing or new) -> save.
 */
test.describe("CMS - Add block to Home", () => {
  test.use(hasAuth() ? { storageState: authPath } : {});

  test.skip("Add Hero Block to Home Hero Area", async ({ page }) => {
    await page.goto("/episerver/cms");

    // TODO: Record with codegen: open Home -> Hero Area -> add block -> save
    await expect(page.getByText(/CMS/i)).toBeVisible();
  });
});
