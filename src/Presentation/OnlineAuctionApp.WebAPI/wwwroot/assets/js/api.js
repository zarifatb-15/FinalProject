const API = {
    baseUrl: '',

    async request(url, options = {}) {
        const config = {
            credentials: 'include',
            ...options,
            headers: {
                ...options.headers
            }
        };

        try {
            const response = await fetch(this.baseUrl + url, config);
            let result;
            
            try {
                result = await response.json();
            } catch (e) {
                if (!response.ok) {
                    throw { isSuccess: false, statusCode: response.status, errors: ['Xəta baş verdi. Server cavab vermir.'] };
                }
                return null;
            }

            if (!result.isSuccess) {
                throw {
                    statusCode: result.statusCode || response.status,
                    errors: result.errors || ['Əməliyyat yerinə yetirilə bilmədi.'],
                    data: result.data
                };
            }

            return result.data;
        } catch (error) {
            if (error.errors) throw error;
            throw { statusCode: 500, errors: [error.message || 'Şəbəkə xətası baş verdi.'] };
        }
    },

    get(url) {
        return this.request(url, { method: 'GET' });
    },

    post(url, data) {
        return this.request(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
    },

    put(url, data) {
        return this.request(url, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
    },

    patch(url, data = null) {
        const options = { method: 'PATCH' };
        if (data) {
            options.headers = { 'Content-Type': 'application/json' };
            options.body = JSON.stringify(data);
        }
        return this.request(url, options);
    },

    delete(url) {
        return this.request(url, { method: 'DELETE' });
    },

    uploadFile(url, file) {
        const formData = new FormData();
        formData.append('image', file);
        return this.request(url, {
            method: 'POST',
            body: formData
        });
    }
};