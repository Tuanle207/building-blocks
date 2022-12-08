import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import i18next from './services/i18n';
import en from './locales/en.json'
import vi from './locales/vi.json'

i18next.init({
  fallbackLng: 'en',
  load: 'languageOnly',
  // backend: {
  //   // loadPath: '/locales/',
  //   loadPath: '/locales/{{lng}}/{{ns}}.json',
  // },
  resources: {
    en,
    vi
  }
});

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
