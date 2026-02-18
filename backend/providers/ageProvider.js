// Simple provider adapter interface (demo + stubs for production providers)

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

module.exports = {
  verifyDob: demo.verifyDob,
  verifyToken: demo.verifyToken,
  verifyWebhookSignature: demo.verifyWebhookSignature,
  supportedProviders: ['demo']
};
