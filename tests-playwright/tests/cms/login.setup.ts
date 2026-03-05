import { test } from "@playwright/test";
import fs from "fs";

/**
 * CMS login setup: run once to save storageState to storage/auth.json.
 * Then use storageState in other CMS tests to avoid logging in every time.
 *
 * Run: npx playwright test tests/cms/login.setup.ts
 * Or:  npm run cms:login
 * Set CMS_USER and CMS_PASS if your credentials differ (default: admin / Password123!)
 */
test("CMS login - save storageState", async ({ page }) => {
  await page.goto("/episerver/cms");

  await page.getByLabel("Username").fill(process.env.CMS_USER || "admin");
  await page.getByLabel("Password").fill(process.env.CMS_PASS || "Password123!");
  await page.getByRole("button", { name: /log in|sign in/i }).click();

  // Wait until we are past the login form (login button disappears = form submitted)
  await page.getByRole("button", { name: /log in|sign in/i }).waitFor({ state: "hidden", timeout: 15_000 });

  await fs.promises.mkdir("storage", { recursive: true });
  await page.context().storageState({ path: "storage/auth.json" });
});
