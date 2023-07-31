// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

self.addEventListener(
    'activate', event => {
        event.waitUntil(
            Promise.all(
                [
                    cleanupLegacyCache(),
                ]
            )
                .catch(
                    (err) => {
                        event.skipWaiting();
                    }
                )
        );
    }
);

//const cacheNamePrefix = 'offline-cache-';
//const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const CACHE_VERSION = '2023.03.28.001'  // Increment each time before deployment.
const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${CACHE_VERSION}`
//const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/ ];


const CACHE_VERSIONS = {
    assets: 'edp-v' + CACHE_VERSION,
    content: 'content-v' + CACHE_VERSION,
    offline: 'offline-v' + CACHE_VERSION,
    notFound: '404-v' + CACHE_VERSION,
};




async function onInstall(event) {
    console.info('Service worker: Install');
 
    // Activate the new service worker as soon as the old one is retired.
    self.skipWaiting();
    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

 
    //// Activate the new service worker as soon as the old one is retired.
    //self.skipWaiting();
    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let cachedResponse = null;
    if (event.request.method === 'GET') {
        // For all navigation requests, try to serve index.html from cache
        // If you need some URLs to be server-rendered, edit the following check to exclude those URLs
        const shouldServeIndexHtml = event.request.mode === 'navigate';

        const request = shouldServeIndexHtml ? 'index.html' : event.request;
        const cache = await caches.open(cacheName);
        cachedResponse = await cache.match(request);
    }

    return cachedResponse || fetch(event.request);
}



function cleanupLegacyCache() {
    let currentCaches = Object.keys(CACHE_VERSIONS).map((key) => {
        return CACHE_VERSIONS[key];
    });

    return new Promise(
        (resolve, reject) => {

            caches.keys().then((keys) => {
                return legacyKeys = keys.filter((key) => {
                    return !~currentCaches.indexOf(key);
                });
            }).then((legacy) => {
                if (legacy.length) {
                    Promise.all(legacy.map((legacyKey) => {
                        return caches.delete(legacyKey)
                    })
                    ).then(() => {
                        resolve()
                    }).catch((err) => {
                        console.error("Error in legacy cleanup: ", err);
                        reject(err);
                    });
                } else {
                    resolve();
                }
            }).catch((err) => {
                console.error("Error in legacy cleanup: ", err);
                reject(err);
            });

        });
}

