/* ── Shared utilities for all pages ───────────────────────── */

// ── Active nav link ──────────────────────────────────────────
(function setActiveNav() {
  const path = window.location.pathname;
  document.querySelectorAll('.nav-link[data-page]').forEach(link => {
    const page = link.dataset.page;
    const match =
      (page === 'home'   && (path === '/' || path === '/index.html')) ||
      (page === 'docs'   && path.includes('docs'))   ||
      (page === 'stats'  && path.includes('stats'))  ||
      (page === 'swagger' && path.includes('swagger'));
    if (match) link.classList.add('active');
  });
})();

// ── Mobile menu toggle ────────────────────────────────────────
const menuToggle = document.getElementById('menuToggle');
const navbarLinks = document.querySelector('.navbar-links');
if (menuToggle && navbarLinks) {
  menuToggle.addEventListener('click', () => {
    navbarLinks.classList.toggle('open');
    menuToggle.textContent = navbarLinks.classList.contains('open') ? '✕' : '☰';
  });
  document.addEventListener('click', e => {
    if (!e.target.closest('.navbar')) {
      navbarLinks.classList.remove('open');
      menuToggle.textContent = '☰';
    }
  });
}

// ── API base URL ──────────────────────────────────────────────
const API = '/api';

// ── Generic fetch with error handling ────────────────────────
async function apiFetch(path) {
  try {
    const res = await fetch(API + path);
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    return await res.json();
  } catch (e) {
    console.warn('API fetch failed:', path, e.message);
    return null;
  }
}

// ── Number formatter ──────────────────────────────────────────
function fmt(n) {
  if (n == null) return '–';
  if (n >= 1_000_000) return (n / 1_000_000).toFixed(1).replace(/\.0$/, '') + 'M';
  if (n >= 1_000)     return (n / 1_000).toFixed(1).replace(/\.0$/, '')     + 'K';
  return String(n);
}

// ── Date formatter ────────────────────────────────────────────
function fmtDate(iso) {
  if (!iso) return '';
  return new Date(iso).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
}

// ── Skeleton helper ───────────────────────────────────────────
function skeleton(lines = 3) {
  return Array.from({ length: lines }, () =>
    `<div class="skeleton skeleton-text" style="width:${60 + Math.random() * 30|0}%"></div>`
  ).join('');
}

// ── Stat card updater ─────────────────────────────────────────
function updateStat(id, value, label) {
  const el = document.getElementById(id);
  if (!el) return;
  el.innerHTML = `
    <div class="stat-value">${fmt(value)}</div>
    ${label ? `<div class="stat-label">${label}</div>` : ''}`;
}

// ── Smooth counter animation ──────────────────────────────────
function animateCount(el, target, duration = 800) {
  const start = performance.now();
  const update = now => {
    const t = Math.min((now - start) / duration, 1);
    const ease = 1 - Math.pow(1 - t, 3);
    el.textContent = fmt(Math.round(ease * target));
    if (t < 1) requestAnimationFrame(update);
    else el.textContent = fmt(target);
  };
  requestAnimationFrame(update);
}

// ── Export for page scripts ───────────────────────────────────
window.PostAPI = { apiFetch, fmt, fmtDate, skeleton, updateStat, animateCount };
