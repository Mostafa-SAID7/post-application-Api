# Documentation Structure

Clean, organized documentation with **zero duplicates**. Each file has a single, clear purpose.

## Root Level Files

**GitHub Standard Files** - Quick reference and community information:

| File | Purpose |
|------|---------|
| `README.md` | Project overview, quick start, links to docs |
| `CONTRIBUTING.md` | Contribution entry point (links to detailed docs) |
| `CHANGELOG.md` | Version history and changes |
| `SECURITY.md` | Security policy and vulnerability reporting |
| `LICENSE` | MIT License |

**Project Files** - Configuration and git:
| File | Purpose |
|------|---------|
| `PostApplication.sln` | Visual Studio solution |
| `.gitignore` | Git ignore rules |
| `.gitattributes` | Git line ending rules |
| `.editorconfig` | Editor formatting rules |
| `.replit` | Replit configuration |

## Docs Folder (`/docs`)

**Comprehensive Developer Documentation** - In-depth guides for specific roles:

| File | Purpose | Audience |
|------|---------|----------|
| `INDEX.md` | Documentation hub & navigation | Everyone |
| `SETUP.md` | Installation, config, troubleshooting | New developers |
| `FEATURES.md` | Feature descriptions & capabilities | Everyone |
| `ARCHITECTURE.md` | Design, patterns, structure | Developers |
| `API.md` | REST endpoints, request/response | API users |
| `CONTRIBUTING.md` | Code standards, patterns, workflow | Contributors |

**Navigation:** Start with [INDEX.md](./INDEX.md)

## GitHub Folder (`/.github`)

**GitHub Configuration** - Automation and organization:

| File | Purpose |
|------|---------|
| `CODEOWNERS` | Code ownership assignment |
| `workflows/` | CI/CD workflows (empty, ready for setup) |

## Hidden Folders

**Agent Memory** (`/.agents/memory`) - Internal context (not for distribution):
- `MEMORY.md` - Agent memory/context
- `static-files-middleware.md` - Archived notes

---

## Organization Principles

### ✅ No Duplicates
- Each concept documented **once**
- Links connect related content
- Root files link to detailed docs

### ✅ Role-Based Navigation
- New developers start at [docs/INDEX.md](./INDEX.md)
- Contributors follow [docs/CONTRIBUTING.md](./CONTRIBUTING.md)
- API users check [docs/API.md](./API.md)

### ✅ GitHub Standards
- Root `README.md` - Project overview
- Root `CONTRIBUTING.md` - Contribution guide
- Root `LICENSE` - License file
- `.github/` - Organization/automation

### ✅ Developer Focus
- All detailed docs in `/docs`
- Concise root files with clear links
- Every file has single purpose

### ✅ Easy Maintenance
- Documentation stays synchronized
- No content scattered across multiple files
- Clear structure for future additions

---

## File Sizes (Optimized)

| File | Size | Purpose |
|------|------|---------|
| README.md | ~2 KB | Quick overview + links |
| CONTRIBUTING.md | ~2 KB | Entry point + links |
| CHANGELOG.md | ~2 KB | Version history |
| SECURITY.md | ~5 KB | Security guidelines |
| docs/INDEX.md | ~3 KB | Documentation hub |
| docs/SETUP.md | ~4 KB | Setup instructions |
| docs/FEATURES.md | ~4 KB | Feature descriptions |
| docs/ARCHITECTURE.md | ~3 KB | Design patterns |
| docs/API.md | ~3 KB | API reference |
| docs/CONTRIBUTING.md | ~11 KB | Detailed standards |

**Total:** ~39 KB - Lightweight, focused documentation

---

## Adding New Documentation

**Principle:** One file = one clear purpose, no duplicates

### When to create a new file:
- ✅ New distinct topic (e.g., "Deployment", "Database Setup")
- ✅ Content useful for specific role only

### When NOT to create a new file:
- ❌ Content duplicates existing documentation
- ❌ Content is a sub-section of existing file
- ❌ Content belongs better as GitHub issue/discussion

### New File Checklist:
1. Check existing files for related content
2. Add to appropriate folder (root for GitHub standard files, `/docs` for dev docs)
3. Update [docs/INDEX.md](./INDEX.md) with link
4. Update root `README.md` if appropriate
5. Keep file focused on single purpose

---

## Quick Links

- **Getting Started:** [docs/SETUP.md](./SETUP.md)
- **Development Standards:** [docs/CONTRIBUTING.md](./CONTRIBUTING.md)
- **Architecture & Design:** [docs/ARCHITECTURE.md](./ARCHITECTURE.md)
- **API Reference:** [docs/API.md](./API.md)
- **All Features:** [docs/FEATURES.md](./FEATURES.md)
- **Contributing:** [CONTRIBUTING.md](../CONTRIBUTING.md)
- **Security:** [SECURITY.md](../SECURITY.md)

---

**Last Updated:** July 2026  
**Status:** ✅ Zero duplicates, fully organized
