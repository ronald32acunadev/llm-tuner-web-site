window.llmTuner = {
  copyToClipboard: async (text) => {
    try {
      await navigator.clipboard.writeText(text);
      return true;
    } catch (e) {
      console.warn('Clipboard write failed, fallbacking', e);
      return false;
    }
  },
  setTheme: (theme) => {
    if (theme === 'system') {
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      document.documentElement.setAttribute('data-theme', prefersDark ? 'dark' : 'light');
    } else {
      document.documentElement.setAttribute('data-theme', theme);
    }
    localStorage.setItem('llmtuner-theme', theme);
  },
  getTheme: () => localStorage.getItem('llmtuner-theme') || 'dark',
  initTheme: () => {
    const saved = localStorage.getItem('llmtuner-theme') || 'dark';
    window.llmTuner.setTheme(saved);
  }
};

window.addEventListener('DOMContentLoaded', () => {
  window.llmTuner.initTheme();
});
