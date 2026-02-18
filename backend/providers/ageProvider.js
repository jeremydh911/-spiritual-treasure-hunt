// Simple provider adapter interface (demo + self‑hosted + stubs for production providers)

const crypto = require('crypto');

// Helper: compute HMAC-SHA256 hex
function hmacHex(secret, payload) {
  return crypto.createHmac('sha256', secret).update(payload).digest('hex');
}

const demo = {
  // dob-based verification (local demo logic)
  verifyDob: (dobStr) => {
    const d = new Date(dobStr);
    if (isNaN(d)) return null;
    const today = new Date();
    let age = today.getUTCFullYear() - d.getUTCFullYear();
    const m = today.getUTCMonth() - d.getUTCMonth();
    if (m < 0 || (m === 0 && today.getUTCDate() < d.getUTCDate())) age--;
    return { age, verified: age >= 18 };
  },

  // token verification (demo): accept token 'demo-verify-true' as verified
  verifyToken: async (providerName, token) => {
    // In production, call the provider's verification API.
    if (providerName === 'demo' && token === 'demo-verify-true') {
      return { verified: true, age: 19, providerId: 'demo-1234' };
    }
    // all other tokens fail for demo
    return { verified: false, age: 0, providerId: null };
  },

  // webhook signature verification - demo always returns true
  verifyWebhookSignature: (providerName, headers, body) => {
    // Production: validate signature/header per provider docs.
    return true;
  }
};

// Self-hosted provider: HMAC-signed tokens and webhook signatures using a shared secret
const self = {
  secret: process.env.SELF_PROVIDER_SECRET || 'dev-self-secret',

  // Token format: `${playerId}:${timestamp}:${hmacHex(secret, `${playerId}:${timestamp}`)}`
  verifyToken: async (providerName, token) => {
    if (providerName !== 'self') return { verified: false };
    try {
      const parts = (token || '').split(':');
      if (parts.length !== 3) return { verified: false };
      const [playerId, tsStr, sig] = parts;
      const payload = `${playerId}:${tsStr}`;
      const expected = hmacHex(self.secret, payload);
      const ts = parseInt(tsStr, 10) || 0;
      const age = null; // self provider won't compute age from token
      // token expiry: 5 minutes
      if (sig !== expected) return { verified: false };
      if (Math.abs(Date.now() - ts) > 5 * 60 * 1000) return { verified: false };
      return { verified: true, age: 99, providerId: `self-${playerId}` };
    } catch (e) {
      return { verified: false };
    }
  },

  // Verify webhook by checking header 'x-self-signature: sha256=<hex>' computed over the JSON body
  verifyWebhookSignature: (providerName, headers, body) => {
    if (providerName !== 'self') return false;
    try {
      const sigHeader = (headers['x-self-signature'] || headers['x-self-signature'.toLowerCase()]);
      if (!sigHeader) return false;
      const parts = sigHeader.split('=');
      if (parts.length !== 2) return false;
      const algo = parts[0];
      const sig = parts[1];
      if (algo !== 'sha256') return false;
      const payload = typeof body === 'string' ? body : JSON.stringify(body);
      const expected = hmacHex(self.secret, payload);
      return crypto.timingSafeEqual(Buffer.from(sig, 'hex'), Buffer.from(expected, 'hex'));
    } catch (e) {
      return false;
    }
  }
};

module.exports = {
  verifyDob: demo.verifyDob,
  verifyToken: async (providerName, token) => {
    if (providerName === 'demo') return demo.verifyToken(providerName, token);
    if (providerName === 'self') return self.verifyToken(providerName, token);
    return { verified: false };
  },
  verifyWebhookSignature: (providerName, headers, body) => {
    if (providerName === 'demo') return demo.verifyWebhookSignature(providerName, headers, body);
    if (providerName === 'self') return self.verifyWebhookSignature(providerName, headers, body);
    return false;
  },
  supportedProviders: ['demo', 'self']
};
