# EDITH DEV STUDIO — GLOBAL GIT BRANCH HYGIENE POLICY

**Effective:** 2026-10-28  
**Scope:** Every EDITH Dev Studio repository and every agent/tool/IDE (Codex, Antigravity, Jules, GitHub, etc.).  
**Authority:** Explicit operator instruction; subordinate to security, code/data integrity, continuity of development, and later explicit operator decisions.

## 1. Branch lifecycle

Keep only the default/main branch and branches still needed for active work, open PRs, supported releases, active deployments, or research with documented unintegrated value.

Every temporary feature, fix, test, documentation, audit, agent, and experiment branch must be removed once its purpose is complete **and deletion has been verified safe**. Do not keep branches solely to preserve history; commits, pull requests, and tags retain the historical record.

Avoid creating duplicate or permanent branches for temporary work; reuse an active branch when appropriate and safe.

## 2. Mandatory post-merge cleanup

After each merged PR:

1. Verify the PR is actually **merged**, not merely closed.
2. Check whether any **open PR uses this branch as its base or otherwise depends on it**.
3. Ensure there are no **unintegrated commits or changes**, including when the integration used squash/rebase. Compare content/patches, not only commit SHAs.
4. Ensure the branch is not required by CI/CD, deployments, release maintenance, an active experiment, another worktree, or recovery.
5. Delete the **remote** source branch when all checks pass, or confirm/report if GitHub already deleted it automatically.
6. Remove stale local references when safe. Distinguish remote-tracking references from a checked-out branch and from linked worktrees.
7. Report the deletion or the concrete reason for retention.

Closing a PR without merging does **not** authorize deletion.

## 3. Existing branches and periodic review

Before each major development phase, and at least monthly, classify branches:

- `ACTIVE`: necessary for ongoing work or an open PR.
- `MERGED`: integrated and potentially removable after safety checks.
- `OBSOLETE`: superseded with no remaining needed content; assess before deletion.
- `REVIEW_REQUIRED`: exclusive commits, unknown state, or dependencies require inspection.
- `PROTECTED`: main/default, protected release/maintenance branch, deployment, or other required dependency.

Automatic deletion is permitted **only** for verified-safe `MERGED` branches.

For `OBSOLETE` or `REVIEW_REQUIRED` branches with exclusive commits, perform technical review and **obtain operator approval** before deletion. If evidence is missing, preserve the branch and report the uncertainty.

Do not interfere with ongoing branch-consolidation audits.

## 4. Non-negotiable safeguards

- Never delete `main`, the default branch, or protected branches.
- Never delete a branch referenced by an open PR, active deployment, CI/CD workflow, active integration base, or required release.
- Never discard unintegrated work without review.
- No force-push as a cleanup shortcut.
- Never merge obsolete code merely to enable branch deletion.
- Examine recovery and backup branches before removal.
- Check dependency order before removing related branches.
- Do not prune worktrees, change checked-out branches, run reset/clean, or remove SQLite files as part of routine branch cleanup.
- Preserve local dirty state, including untracked files, and document blocked cleanup.

## 5. Agent responsibility

An agent that creates a branch is responsible for its lifecycle: create only when necessary, use a descriptive task-linked name, avoid duplicates, implement and test, review and merge as authorized, and perform verified-safe cleanup. If interrupted, record the work state, PR, dependencies, and whether the branch remains necessary.

## 6. Permitted exceptions

Retain main/default, active integration/development, open-PR, supported release/maintenance, deployment, and research branches with documented unintegrated value. Every exception needs a concrete rationale.

## 7. Mandatory reporting

Conclude each GitHub-related activity with a compact **GIT BRANCH HYGIENE** section:

- **Branch utilizzato:**
- **PR associata:**
- **Stato del merge:**
- **Branch remoto eliminato:** SÌ / NO
- **Se conservato, motivazione:**
- **Eventuali branch obsoleti individuati:**

The check should be proportional to the task; a full audit is not required after every commit.

## 8. Immediate application

- Start cleanup with fully integrated, demonstrably unnecessary branches.
- Preserve valuable work and avoid disrupting existing consolidation audits.
- Mark suspicious or abandoned branches for review rather than assuming they can be deleted.
- Make post-merge cleanup part of every future development task, not a delayed maintenance project.

> A branch exists because it serves current development, not merely because it existed in the past. Minimize branch count while preserving every unintegrated change.
