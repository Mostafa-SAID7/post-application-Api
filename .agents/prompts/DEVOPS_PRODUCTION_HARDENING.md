# DevOps Production Hardening & Release Automation Prompt

## Role

Act as a **Principal DevOps / Platform Engineer and senior .NET CI/CD reviewer**. Work directly against this repository and implement a production-grade GitHub engineering system without changing application behavior unnecessarily.

Repository: `Mostafa-SAID7/post-application-Api`
Default branch: `main`
Target runtime: ASP.NET Core / .NET 8
Architecture: Clean Architecture + CQRS + EF Core

The repository currently has a `.github/CODEOWNERS` file but no `.github/workflows` directory, and it has a `CHANGELOG.md` that documents Semantic Versioning and currently lists `1.1.0` and `1.0.0`. There are currently no GitHub Releases. Treat the existing version history as authoritative context, but do **not** create a fake release merely to make automation appear complete.

## Primary Objective

Bring the repository to a **professional, maintainable, secure, deterministic CI/CD and release baseline** suitable for an enterprise .NET API and strong enough for portfolio/code-review evaluation.

Do not merely add a YAML file. Audit the repository first, then implement the complete DevOps lifecycle:

`PR -> validation -> build/test/security gates -> merge -> release decision -> semantic version -> annotated Git tag -> GitHub Release -> changelog -> deploy-ready artifact -> post-release verification`

The implementation must be reproducible, least-privilege, fail-fast, and understandable by another engineer.

---

# 1. Repository Discovery — MUST HAPPEN FIRST

Before modifying anything:

1. Inspect the complete repository tree.
2. Inspect all existing `.github` files.
3. Inspect all `.csproj` files and the solution.
4. Determine whether test projects actually exist.
5. Inspect `Program.cs`, configuration files, EF Core setup/migrations, and startup behavior.
6. Inspect existing documentation, `CHANGELOG.md`, `SECURITY.md`, and `CONTRIBUTING.md`.
7. Inspect existing Git history and existing tags/releases if accessible.
8. Check whether Docker/container deployment already exists. Do not invent an application container contract without verifying the project.
9. Identify the real application health endpoint(s). If no health endpoint exists, explicitly report that and decide whether adding a minimal `/health` endpoint is appropriate.
10. Identify the real publish output and runtime requirements.

Never assume a file, script, test project, Dockerfile, health endpoint, database migration strategy, or deployment target exists.

---

# 2. Current-State Assessment

Produce an internal checklist with findings grouped as:

- 🔴 Critical
- 🟠 High
- 🟡 Medium
- 🟢 Good

At minimum assess:

- CI coverage
- PR validation
- build reproducibility
- test coverage / absence of tests
- dependency consistency
- .NET SDK pinning
- NuGet dependency restore determinism
- formatting / analyzers
- vulnerability scanning
- secret scanning
- workflow permissions
- concurrency
- artifact handling
- release automation
- Semantic Versioning
- Conventional Commits
- changelog generation
- Git tags
- GitHub Releases
- branch protection assumptions
- CODEOWNERS correctness
- deployment readiness
- rollback strategy
- environment separation
- database migration safety
- observability / health checks
- documentation

Do not hide gaps. If a capability cannot safely be automated because credentials, infrastructure, or repository settings are unavailable, document it as an explicit external prerequisite instead of fabricating it.

---

# 3. .NET Build Standardization

Implement deterministic CI for the actual solution.

Requirements:

- Use a supported .NET 8 SDK.
- Pin the SDK with `global.json` unless the repository already has an equivalent deterministic mechanism.
- Use `dotnet restore`.
- Use `dotnet build --no-restore` in CI after restore.
- Use Release configuration for production validation.
- Run `dotnet test` when test projects exist.
- If no test projects exist, do NOT fake a passing test step. Make the workflow clearly report that automated tests are currently unavailable and create an actionable follow-up/documentation item.
- Run `dotnet publish` as a separate production-artifact validation step.
- Use NuGet caching through supported `actions/setup-dotnet` mechanisms where appropriate.
- Prefer locked/dependency-deterministic restore where compatible with the repository. If introducing lock files, validate the repository actually supports them before enforcing them.
- Do not use floating SDK/package versions in newly introduced DevOps configuration.

If package versions are inconsistent with the target framework, flag and fix them only when the fix is technically safe and verified by build/test.

---

# 4. Quality Gates

The CI pipeline should validate, as applicable:

1. Restore
2. Build
3. Tests
4. Code formatting / analyzers
5. Vulnerable NuGet dependencies
6. Publish
7. Optional static/security analysis

Do not add noisy checks that have no signal. Every gate must have a reason and a failure message that helps the maintainer remediate the problem.

For formatting, use the repository's actual formatting conventions. If `dotnet format` is introduced, use it in verification mode rather than mutating files inside CI.

For vulnerability scanning, use a maintainable command such as `dotnet list package --vulnerable --include-transitive` or the modern equivalent supported by the pinned SDK. Make the workflow fail on actionable vulnerabilities rather than silently printing them.

---

# 5. GitHub Actions Architecture

Create a clean `.github/workflows` structure. Prefer small workflows with single responsibilities over one giant workflow.

Recommended baseline:

- `ci.yml` — PR + main validation
- `codeql.yml` — CodeQL security analysis for C#
- `dependency-review.yml` — dependency review on pull requests when supported
- `release.yml` — controlled semantic release + tag + GitHub Release
- `publish.yml` or deployment workflow only if a real deployment target and credentials are known

Do not create deployment automation that cannot actually authenticate to the target environment.

All workflows MUST:

- declare explicit `permissions`
- default to least privilege
- use `actions/checkout` with a pinned major version or safer supported reference
- use `actions/setup-dotnet`
- define `timeout-minutes`
- use `concurrency` where duplicate runs can waste resources
- avoid plaintext secrets
- avoid printing secret values
- avoid `pull_request_target` unless there is a compelling, documented reason
- avoid arbitrary third-party actions unless necessary
- use immutable/pinned action references where practical for a production-grade baseline
- use clear job names
- expose enough logs to diagnose failures without leaking sensitive data

For forked PRs, never grant write permissions or expose repository secrets.

---

# 6. CI Workflow Design

Build a PR-safe pipeline that runs on:

- `pull_request`
- `push` to `main`

Recommended job structure:

`validate -> build -> test -> security/dependency checks -> publish artifact`

Where possible, make independent checks parallel, but keep the final quality gate explicit.

The workflow must not depend on local developer state.

Use `--no-restore` only after a successful restore step.

Cache NuGet packages safely and avoid caching build outputs that could cause stale or cross-commit behavior.

Upload the publish output as an artifact on successful main builds if it provides value for release/deployment workflows. Use retention appropriate for a small open-source project.

---

# 7. Semantic Versioning — VERY IMPORTANT

Implement a deterministic release strategy based on **Conventional Commits** and **Semantic Versioning 2.0.0**.

Rules:

- `fix:` -> PATCH
- `feat:` -> MINOR
- `BREAKING CHANGE:` footer or `!` -> MAJOR
- `docs:`, `refactor:`, `test:`, `chore:`, `ci:`, `build:` normally do not create a release by themselves
- merge commits and arbitrary commit text must not produce accidental versions

The release automation must:

1. Determine the latest existing semantic version tag.
2. Determine commits since that tag.
3. Calculate the next semantic version deterministically.
4. Validate the version is greater than the previous version.
5. Generate/update release notes/changelog from actual commits.
6. Create an annotated Git tag in the form `vX.Y.Z`.
7. Create a GitHub Release associated with exactly that tag.
8. Prevent duplicate releases/tags.
9. Be safe to re-run after partial failure.
10. Never silently overwrite an existing release or move an existing release tag.

If the repository currently has no tags but `CHANGELOG.md` contains historical versions, treat those historical versions as history only. Do not manufacture a tag pointing to an unrelated old commit unless there is explicit evidence that doing so is intended.

For the first automated release, establish a clear baseline strategy and document it.

Prefer a well-maintained release automation tool/action that supports Conventional Commits and changelog generation, but keep the implementation transparent. If using a third-party release action, pin it and document why it is used.

---

# 8. Release Safety

Release automation MUST NOT run for every arbitrary push.

Use one controlled strategy, preferably:

- CI must pass on `main`.
- Release job runs only from `main`.
- Release job calculates whether a release is needed.
- If no releasable Conventional Commit exists, exit successfully without creating a release.
- If a release is needed, create exactly one version/tag/release.

Use concurrency to prevent two release runs from racing.

Use GitHub's built-in `GITHUB_TOKEN` wherever sufficient. Request only the permissions required to create releases/tags.

Never store a personal access token in the repository when `GITHUB_TOKEN` is sufficient.

---

# 9. Conventional Commit Enforcement

Add commit/PR validation where useful.

The system should encourage:

- `feat:`
- `fix:`
- `docs:`
- `refactor:`
- `test:`
- `perf:`
- `build:`
- `ci:`
- `chore:`
- `revert:`

Support scoped forms such as `feat(api): ...`.

Validate breaking changes correctly.

Do not make contributor UX unnecessarily painful. If commit-level enforcement is difficult because contributors squash PRs, enforce **PR title Conventional Commits** instead and document the squash strategy.

Choose one source of truth for release semantics; do not create competing version calculators.

---

# 10. CHANGELOG Strategy

The repository already claims Keep a Changelog + Semantic Versioning. Preserve that intent unless there is a compelling reason to migrate.

Do not maintain two conflicting changelog systems.

If automated release notes are generated by GitHub, ensure they are compatible with the repository's changelog expectations.

Document clearly:

- how versions are calculated
- how release notes are generated
- when `CHANGELOG.md` is updated
- whether changelog updates are committed automatically or generated as release notes
- how breaking changes are represented

Avoid an automation loop where release-generated commits trigger another release.

---

# 11. Git Tags and Releases

Target convention:

- Tag: `v1.2.3`
- Release name: `v1.2.3`
- Release: generated from the same tag
- Release notes: generated from commits since the previous release

The release workflow must verify:

- tag does not already exist
- release does not already exist
- version is valid SemVer
- tag points to the intended commit
- release is not created from a failing CI state

Use annotated tags when the mechanism supports them.

Never force-move release tags.

---

# 12. Security Hardening

Audit and improve GitHub Actions security.

Minimum:

```yaml
permissions:
  contents: read
```

Then grant write permissions only to the specific release job that needs them.

For example, release jobs may require:

```yaml
permissions:
  contents: write
```

Do not grant write permissions globally if only one job needs them.

Also consider:

- CodeQL
- dependency review
- NuGet vulnerability scanning
- secret scanning compatibility
- Dependabot configuration
- action pinning
- minimal token permissions
- fork PR safety
- artifact integrity
- no credential logging

Do not attempt to modify GitHub account-level secret-scanning settings through repository files.

---

# 13. Dependabot

Add `.github/dependabot.yml` if missing.

Configure appropriate ecosystems for the repository, at minimum NuGet and GitHub Actions if applicable.

Keep update noise manageable:

- weekly schedule is acceptable
- group compatible dependency updates where practical
- keep major .NET framework upgrades reviewable
- do not automatically merge risky major updates

Do not invent ecosystems that do not exist in this repository.

---

# 14. CODEOWNERS

The current CODEOWNERS file is extremely minimal. Inspect it and improve it only if ownership rules can be made accurate.

Do not invent maintainers or teams.

If the authenticated GitHub owner is the only known maintainer, use the verified repository owner rather than an unknown username/team.

Ensure workflow changes have an appropriate owner rule if practical.

Example category to consider:

- `.github/**`
- source/application directories
- infrastructure
- documentation

But only add rules backed by real repository ownership.

---

# 15. Pull Request Governance

If repository settings can be configured externally, document recommended branch protection for `main`:

- require pull requests
- require CI checks
- require conversation resolution
- require CODEOWNERS review where appropriate
- block force pushes
- block branch deletion if desired
- require linear history if compatible with the team's merge strategy
- enable automatic branch deletion after merge

Do not claim these settings are enabled merely because documentation recommends them.

Repository configuration and workflow code are separate concerns.

---

# 16. Docker / Containerization

First determine whether the application already has a Dockerfile.

If absent, decide whether containerization is justified for the repository. If adding it:

- use a multi-stage build
- use the official .NET 8 runtime image for the final image
- run as a non-root user where supported
- avoid copying secrets
- use `.dockerignore`
- expose only the actual application port
- use a health check only if a real health endpoint exists
- make the image reproducible
- avoid unnecessary packages
- validate with `docker build`

Do not add Docker merely for appearance if it complicates the actual deployment model.

---

# 17. Database / EF Core Deployment Safety

The repository uses EF Core and database providers. Audit migrations and startup database behavior.

Do not blindly execute `dotnet ef database update` against production from CI.

Prefer:

- migration script generation
- reviewed database changes
- environment-specific execution
- backup/rollback planning

If the application auto-applies migrations at startup, flag that as a production deployment consideration and only change it if you can preserve development behavior safely.

Document the production migration strategy.

---

# 18. Health / Observability

Verify whether the API has:

- health endpoint
- readiness/liveness distinction where needed
- structured logging
- request correlation
- exception handling

If the application already has suitable middleware, do not duplicate it.

For deployment readiness, a lightweight `/health` endpoint is preferred when absent, but only add it if it fits the existing architecture cleanly.

Do not add an entire observability platform just for the sake of the checklist.

---

# 19. Deployment Strategy

The repository currently advertises a RunASP-hosted application. Do not fabricate deployment credentials or a provider-specific workflow.

Instead:

1. Make CI and release artifacts deployment-ready.
2. Document the required deployment secrets/environment variables if the target is known.
3. If RunASP deployment requires FTP/WebDeploy/API credentials that are not available, create a clearly documented deployment interface/template rather than a workflow that will always fail.
4. Keep production deployment separate from release creation when possible.
5. Prefer GitHub Environments such as `staging` and `production` for future deployment.
6. Require approval for production deployment when supported.

If a real deployment mechanism is discovered in the repository, integrate with it instead of replacing it.

---

# 20. Artifact Strategy

A successful production build should create a deterministic publish artifact.

Requirements:

- artifact name should include application and version/commit context where practical
- artifact should contain only deployable output
- do not include `.git`, source control metadata, secrets, local settings, or development-only files
- artifact retention should be explicit
- release/deployment workflow should consume the artifact rather than rebuilding different source when feasible

---

# 21. Documentation

Create or update a focused DevOps/release guide, preferably:

`docs/DEVOPS.md`

It must explain:

- local validation commands
- CI workflow map
- security checks
- Conventional Commits
- version calculation
- tag convention
- release process
- changelog strategy
- artifact lifecycle
- deployment prerequisites
- secrets/environment variables
- database migration strategy
- rollback procedure
- how to recover from failed release automation
- how to manually trigger/recover safely if workflow dispatch is supported

Keep documentation concise and non-duplicative.

---

# 22. Recommended Repository Files

Depending on what discovery shows, the final repository may contain:

```text
.github/
├── CODEOWNERS
├── dependabot.yml
├── pull_request_template.md
├── workflows/
│   ├── ci.yml
│   ├── codeql.yml
│   ├── dependency-review.yml
│   └── release.yml

.agents/
└── prompts/
    └── DEVOPS_PRODUCTION_HARDENING.md

docs/
└── DEVOPS.md

global.json
```

Only add files that are justified by the audit.

---

# 23. Workflow Quality Rules

Every workflow must be reviewed for:

- valid YAML
- correct event triggers
- correct branch filters
- correct job dependencies
- correct permissions
- correct .NET version
- correct solution path
- correct working directory
- cache correctness
- artifact correctness
- shell portability
- failure behavior
- rerun behavior
- concurrency
- secret safety
- fork PR safety

Do not assume a workflow is valid because it looks syntactically correct. Validate the commands against the actual repository structure.

---

# 24. Release Algorithm — Preferred Behavior

Implement the following logical algorithm:

```text
on push to main
    run full CI
    if CI fails:
        stop

    acquire release concurrency lock

    find latest vMAJOR.MINOR.PATCH tag

    if no tag exists:
        establish documented baseline
        do not infer a historical tag without evidence

    collect commits since latest release

    classify Conventional Commits

    if no releasable commits:
        exit successfully with "no release required"

    calculate next SemVer

    verify tag does not exist
    verify release does not exist

    generate release notes

    create annotated tag
    create GitHub Release

    publish deployment artifact / hand off artifact

    report version + tag + release URL
```

The implementation may use a proven release tool instead of hand-written shell parsing, but the observable behavior must follow this model.

---

# 25. Failure Recovery

Design for partial failures:

### CI failure
No release.

### Release calculation failure
No tag/release.

### Tag creation succeeds but release creation fails
A rerun must detect the existing tag and safely create the missing release without moving the tag.

### Release exists but workflow fails afterward
A rerun must not create a second release.

### Two release runs start simultaneously
Concurrency must prevent duplicate version calculation/tagging.

### No releasable commits
Successful no-op.

Never use `git push --force` for release tags.

---

# 26. Validation Commands

After implementation, execute as much of the following as the environment allows:

```bash
dotnet --info
dotnet restore PostApplication.sln
dotnet build PostApplication.sln --configuration Release --no-restore
dotnet test PostApplication.sln --configuration Release --no-build

dotnet publish Post.Api/Post.Api.csproj --configuration Release --no-build

dotnet list PostApplication.sln package --vulnerable --include-transitive
```

If a command is not applicable, explain why instead of hiding it.

Also validate workflow YAML structurally and inspect every introduced workflow for security and logical errors.

---

# 27. Acceptance Criteria

Do not consider the task complete until all applicable criteria are satisfied:

- [ ] Repository audited before changes
- [ ] CI exists and validates PRs
- [ ] Main branch validation exists
- [ ] .NET SDK is deterministic/pinned
- [ ] Restore/build/test/publish strategy is correct
- [ ] Missing tests are explicitly handled, never faked
- [ ] NuGet vulnerability scanning exists
- [ ] CodeQL exists if appropriate
- [ ] Dependency review exists if appropriate
- [ ] Dependabot exists if appropriate
- [ ] Workflow permissions are least-privilege
- [ ] Concurrency is configured where needed
- [ ] No secrets are hardcoded
- [ ] Conventional Commit strategy is documented/enforced appropriately
- [ ] Semantic Versioning strategy is deterministic
- [ ] Release workflow creates `vX.Y.Z` tags
- [ ] Release workflow creates GitHub Releases
- [ ] Duplicate tags/releases are prevented
- [ ] Release workflow is safe to rerun
- [ ] Changelog strategy is documented and non-conflicting
- [ ] Deployment is artifact-driven or explicitly documented as external
- [ ] Database migration strategy is documented
- [ ] CODEOWNERS is accurate and does not invent owners
- [ ] DevOps documentation exists
- [ ] All workflows are internally consistent
- [ ] Final validation has been performed

---

# 28. Final Deliverable

At the end, provide a concise implementation report containing:

1. **Current state** — what was missing/broken.
2. **Implemented** — exact files added/changed.
3. **Release model** — exact Conventional Commit -> SemVer rules.
4. **Tag model** — exact tag convention.
5. **CI flow** — PR/main/release sequence.
6. **Security** — permissions, scanning, dependency strategy.
7. **Deployment** — what is automated and what remains infrastructure-dependent.
8. **Validation results** — exact commands and pass/fail status.
9. **Remaining external prerequisites** — GitHub settings, secrets, environments, hosting credentials, etc.
10. **Rollback/recovery procedure** — how to safely recover from a failed release.

Do not claim "production ready" merely because workflows were added. State remaining infrastructure-level prerequisites explicitly.

## Non-Negotiable Engineering Principles

- No fake tests.
- No fake deployment.
- No hardcoded secrets.
- No force-moving release tags.
- No duplicate version sources.
- No accidental release on non-release commits.
- No broad GitHub token permissions.
- No unexplained third-party actions.
- No undocumented manual release steps.
- No destructive repository history rewriting.
- Prefer deterministic, boring, maintainable automation over clever YAML.
