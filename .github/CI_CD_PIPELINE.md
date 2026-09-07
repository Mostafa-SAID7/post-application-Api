# CI/CD Pipeline Documentation

## Overview

This project implements a **production-grade CI/CD pipeline** with:
- ✅ Conventional commit validation
- ✅ Semantic versioning (SemVer)
- ✅ Automated releases with GitHub tags
- ✅ Security scanning (CodeQL + dependency scanning)
- ✅ Deterministic builds
- ✅ Dependency management (Dependabot)

## Release Lifecycle

The release process is **fully automated** and enforced:

```
Code Commit (conventional commits)
    ↓
PR Validation (conventional commit format, secret detection)
    ↓
CI Pipeline (build, test, format, security scans)
    ↓
Merge to main
    ↓
Semantic Release (auto-calculate version)
    ↓
Git Tag (vX.Y.Z - immutable annotated tag)
    ↓
GitHub Release (with release notes)
    ↓
Artifact Publishing (deployable binaries)
```

## Workflows

### 1. PR Validation (`pr-validation.yml`)

**Triggers:** Pull requests and pushes to main

**Features:**
- ✅ Conventional commit validation (feat, fix, docs, chore, etc.)
- ✅ Secret detection (TruffleHog)
- ✅ No secrets are checked into repository

**Permissions:** `read-only`

### 2. CI Pipeline (`ci.yml`)

**Triggers:** Pull requests and pushes to main

**Steps:**
1. **Restore & Build** - Compile Release configuration
2. **Format & Analyzers** - Code style, warnings as errors
3. **Dependency Check** - Vulnerable and outdated packages
4. **Publish Validation** - Dry-run publish to verify artifacts
5. **CI Summary** - Overall pipeline status

**Permissions:** `read` + `security-events: write`

### 3. CodeQL Security (`codeql.yml`)

**Triggers:** Push to main, PRs, nightly at 2 AM UTC

**Features:**
- ✅ Static analysis for C# code
- ✅ Security vulnerability detection
- ✅ Code quality analysis
- ✅ Nightly automated scans

**Configuration:** `.github/codeql-config.yml` (excludes tests/build artifacts)

**Permissions:** `read` + `security-events: write`

### 4. Semantic Release (`semantic-release.yml`)

**Triggers:** Push to main (after CI passes)

**Safety Checks:**
- ✅ Duplicate tag detection
- ✅ CI verification gate
- ✅ Immutable annotated tags
- ✅ Single source of truth for version

**Version Calculation:**
- `feat:` → MINOR version bump
- `fix:` → PATCH version bump
- `BREAKING CHANGE:` → MAJOR version bump

**Permissions:** `contents: write` (release artifacts only)

### 5. Dependabot (`dependabot.yml`)

**Features:**
- ✅ NuGet updates: Weekly (Monday 3 AM UTC)
- ✅ GitHub Actions updates: Weekly (Monday 4 AM UTC)
- ✅ Automatic rebasing
- ✅ Pre-release exclusion
- ✅ Conventional commit prefixes
- ✅ Automatic reviewer assignment

## Version Management

### Single Source of Truth

**File:** `Directory.Build.props`

```xml
<VersionPrefix>1.1.0</VersionPrefix>
```

All project files inherit this version. **Never hardcode versions in .csproj files.**

### Version Synchronization

```
Directory.Build.props (source)
    ↓ (inherited by)
Post.Api.csproj
Post.Domain.csproj
Post.Application.csproj
Post.Infrastructure.csproj
    ↓ (compiled into)
Assembly Version (1.1.0.0)
```

### Release Flow

1. Commit with `feat:`, `fix:`, or `BREAKING CHANGE:`
2. CI runs → passes
3. Merge to main → semantic-release.yml triggers
4. Version calculated from commits
5. Tag created: `v1.2.0`
6. GitHub Release published
7. Artifacts uploaded

## Security Features

### Prevented Anti-Patterns

❌ **Duplicate tags** - Detection prevents tag overwrite
❌ **Duplicate releases** - Each version releases once
❌ **Accidental releases from docs/chore** - Only feat/fix/BREAKING trigger release
❌ **Release on CI failure** - CI verification gate required
❌ **Release from non-main** - main branch only
❌ **Secrets in workflow** - TruffleHog detection + secret masking
❌ **Overpermissive permissions** - `write-all` never used
❌ **Unpinned actions** - All actions pinned to commit hashes
❌ **Race conditions** - Sequential job dependencies

### Action Pinning

All GitHub Actions are pinned to **commit hashes** (not tags):

```yaml
# ✅ CORRECT (commit hash)
uses: actions/checkout@eacf72885be6976f31d90d3390724caa597ea75a # v4.1.0

# ❌ WRONG (unpinned)
uses: actions/checkout@v4
uses: actions/checkout@main
```

## Deterministic Builds

Build configuration ensures **reproducible artifacts**:

```xml
<Deterministic>true</Deterministic>
<EmbedUntrackedSources>true</EmbedUntrackedSources>
<PublishRepositoryUrl>true</PublishRepositoryUrl>
```

Benefits:
- Same source = same binary
- Enables artifact verification
- Supports reproducible deployments

## Conventional Commits

### Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- `feat` - Feature (MINOR version bump)
- `fix` - Bug fix (PATCH version bump)
- `docs` - Documentation
- `style` - Code style changes
- `refactor` - Code refactoring
- `perf` - Performance improvements
- `test` - Test additions/changes
- `chore` - Build/tooling changes
- `ci` - CI/CD changes
- `build` - Build system changes

### Scopes (Optional)

- `api` - API layer
- `app` - Application logic
- `infrastructure` - Infrastructure layer
- `domain` - Domain logic
- `db` - Database changes
- `auth` - Authentication
- `release` - Release-related

### Example

```
feat(api): add post search with filters

- Add advanced search endpoint
- Support filtering by category and tags
- Implement pagination

Closes #42
```

## Developer Workflow

### Local Development

1. Create feature branch:
   ```bash
   git checkout -b feat/my-feature
   ```

2. Make changes with conventional commits:
   ```bash
   git commit -m "feat(api): add new endpoint"
   ```

3. Push and create PR:
   ```bash
   git push -u origin feat/my-feature
   ```

4. PR checks run automatically:
   - Conventional commit validation
   - Secret detection
   - CI pipeline

5. Merge to main once approved

6. Semantic release automatically triggers:
   - Version calculated
   - Tag created
   - Release published

### Gitpod Development

Contributors can develop in cloud:

```
Open: https://gitpod.io/#https://github.com/mohammedhossam3300-ctrl/post-application-Api
```

Includes:
- .NET 8 SDK pre-installed
- VS Code extensions configured
- Database migrations auto-applied
- Port 5000 auto-opens

## Troubleshooting

### PR Validation Fails

- ❌ Commit format invalid
- ✅ Use conventional commits: `feat(scope): description`

- ❌ Secrets detected
- ✅ Check commit for hardcoded passwords/keys
- ✅ Use GitHub Secrets for sensitive data

### CI Build Fails

- ❌ Code format issues
- ✅ Run locally: `dotnet format`

- ❌ Compilation errors
- ✅ Check error logs in Actions

- ❌ Vulnerable dependencies
- ✅ Update packages: `dotnet package update`

### Release Doesn't Trigger

- ❌ Commits after last tag are not feat/fix/BREAKING
- ✅ Use conventional commits to trigger release

- ❌ Push to non-main branch
- ✅ Releases only trigger from main

- ❌ CI failed on this commit
- ✅ Fix CI errors first

## Maintenance

### Update Actions

Dependabot automatically creates PRs for action updates (Monday 4 AM UTC).

Review and merge to keep workflows current.

### Update Dependencies

NuGet updates: Weekly on Monday 3 AM UTC.

- Review Dependabot PRs
- Merge after verification
- CI runs automatically

### Monitor Security

- CodeQL scans nightly (2 AM UTC)
- Review findings in GitHub Security tab
- Address high-severity issues immediately

## References

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)
- [GitHub Actions Security](https://docs.github.com/en/actions/security-guides)
- [CodeQL Documentation](https://codeql.github.com/docs/)

---

**Last Updated:** 2026-09-07
**Status:** ✅ Production-Ready
