# Migrating a Connector/Enricher to Multi-Version Targeting

This document tracks the migration of `CluedIn.Enricher.BvD` from a single-version build to the
multi-version targeting pattern. Part of a wider effort that has already migrated
`CluedIn.Connector.Dataverse.V2`, `CluedIn.Enricher.Gleif`, `.OpenCorporates`, `.Permid`, `.Brreg`,
`.KnowledgeGraph`, `.ClearBit`, `.CompanyHouse`, `.CVR`, `.DuckDuckGo` (all consulted directly).

---

## Overview

| CluedIn version | .NET TFM | Package suffix |
|---|---|---|
| 4.7.0 | net6.0 | `.470` |
| 4.8.0 | net6.0 | `.480` |
| 5.0.0-beta.* | net10.0 | `.500` |

Verified independently for this repo's own feeds: `5.0.0-*` resolves to `5.0.0-beta.576`.
`CluedIn.Core` at `4.7.0`/`4.8.0` restores cleanly against this repo's existing feeds (develop /
release / AzurePipelines / nuget.org) — no extra "public" feed needed, unlike AzureEventHubs.

4.6.0 excluded — this is a plain `IExternalSearchProvider` (`CluedIn.Core`, `CluedIn.ExternalSearch`,
`CluedIn.Integration.PrivateServices` only), no stream-repository usage that would require it.

Branch: `feature/multi-version-targeting` (off `origin/develop`; local `develop` was stale, fetched
first).

---

## Step 1 — Pipeline template (`azure-pipelines.yml`)

Status: **Done**

Switched from `crawler.build.yml` (steps-template, with an explicit `UseDotNet@2` installing 8.0.x —
stale, unrelated to the repo's actual net10.0 default) to `crawler.build.jobs.yml`
(`multiVersionCluedInTargets`: 4.7.0/4.8.0/5.0.0-beta.*). `pipelineTemplateRef` fixed to
`refs/heads/feature/multi-version-packaging`.

Removed `createIntegrationEnvironmentScriptFilePath`/`Arguments` (pointed at
`./build/integration-test.ps1`, which doesn't exist in this repo — same dead reference GoogleMaps'
doc found in its own repo). `runIntegrationTests` default kept at `false` (unchanged from before);
the one integration test class (`BvDTests.cs`) is entirely gated behind `#if BVD_DEV` (never
defined) so only a placeholder `Dummy` test actually runs either way — no real environment/API-key
dependency either way.

---

## Step 2 — `Directory.Build.props`

Status: **Done**

Same pattern as every prior repo: honours `CluedInMultiVersionTargetFramework` (net10.0 local
fallback), derives `CLUEDIN_V47`/`V48`/`V50` `DefineConstants`, pins `LangVersion` to `13.0`.

---

## Step 3 — `Packages.props`

Status: **Done**

`_CluedIn` guarded. Test package versions split by `CLUEDIN_V50` (xunit v3/AutoFixture.Xunit3 vs
xunit v2/AutoFixture.Xunit2 — the repo already had test projects for both `unit` and `integration`,
unlike Dataverse.V2). `CluedIn.Testing.Base` referenced via the interpolated suffixed package ID
(`CluedIn.Testing.Base.$(_CluedInPackageSuffix)`) per the GoogleMaps precedent — confirmed on the
feed the suffixed packages (`.470`/`.480`/`.500`, all currently at `1.0.0-pr0013.2`) exist before
wiring this up. The integration test csproj's own `PackageReference Include` was updated to match
the same interpolated ID (central package management's `Update` only affects an already-`Include`d
item with the same spec).

`NuGet.config` — renamed from `Nuget.config` (two-step `git mv`, Windows case-insensitivity).

---

## Step 4 — Test projects

Status: **Done**

`test/Directory.Build.props` — package refs split by `CLUEDIN_V50` (same pattern as every prior
repo). No `GlobalUsings.cs` needed — neither `Dummy.cs` test file uses `AutoFixture` or
`ITestOutputHelper`, only a bare `[Fact]`/`using Xunit;`, which works identically under both xunit
generations.

---

## Step 5 — API compatibility audit across 4.7.0 / 4.8.0 / 5.0.0-beta.*

Status: **Done**

Built and ran the full solution (both src projects, both test projects) for real against all three
legs — 0 compile errors, all tests pass, after two fixes:

1. **RestSharp 106-vs-114 break** (same family every repo in this effort has hit) in
   `BvDExternalSearchProvider.cs`:
   - `Method.Post` doesn't exist pre-107 (legacy all-caps `Method.POST` only) — centralized into one
     `HttpPostMethod` const guarded by `#if CLUEDIN_V50`, used at all 4 call sites, rather than
     repeating the `#if` inline each time.
   - `ConstructVerifyConnectionResponse<T>(RestResponse<T> response)` — the parameter type is a
     concrete `RestResponse<T>` only from RestSharp 107+; pre-107 `ExecuteAsync<T>()` returns
     `IRestResponse<T>`, which isn't assignable to the concrete type. Guarded the method signature
     itself (`RestResponse<T>` under `CLUEDIN_V50`, `IRestResponse<T>` otherwise) — both call sites
     pass a `var`-inferred local, so no change needed at the call sites themselves.

Verified with real `dotnet build` (0 errors) **and** `dotnet test` (all Dummy tests pass) on all
three legs, not just build.

---

## Step 6 — Reset the semantic version (`GitVersion.yml`)

Status: **Done**

This repo's `GitVersion.yml` already had an `ignore: sha: []` block — merged `commits-before` into
it rather than adding a second top-level `ignore:` key (CompanyHouse and CVR both found a second key
silently clobbers the first, no YAML error, zero effect).

```yaml
next-version: 1.0
ignore:
  sha: []
  commits-before: 2026-06-20T00:00:00
```

Highest pre-existing tag is `4.7.1` at `2026-06-17T17:26:48+10:00`; padded to `2026-06-20T00:00:00`
(2+ days past it, matching the margin every repo in this effort has needed since
`GitVersion.Tool 5.9.0` appears to compare `commits-before` against local machine time, not UTC).

**New gotcha for local GitVersion verification (not a CI issue, a local-testing one):** running the
pinned `dotnet-gitversion 5.9.0` against `feature/multi-version-targeting` threw
`System.InvalidOperationException: Gitversion could not determine which branch to treat as the
development branch` — even after committing. Root cause: this repo (like several others cloned a
while ago) only had `origin/develop` as a remote-tracking ref, no **local** branch literally named
`develop` (`git branch -a` showed no plain `develop` row, only `remotes/origin/develop`).
GitVersion's branch-config inheritance apparently requires a local branch matching the configured
`develop` name to classify a `feature/*` branch's parent correctly. Fixed by creating one:
`git branch develop origin/develop` (never checked out, purely so GitVersion can see it) — verified
`MajorMinorPatch: "1.0.0"` immediately after. Real CI checks out branches differently and is
unaffected; this only matters when verifying GitVersion locally on a repo whose local clone predates
`develop` existing as a normal local branch.

