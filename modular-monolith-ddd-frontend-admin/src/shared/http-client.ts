import { ApplicationSettings } from "../configuration/application-settings";
import { getAccessToken } from "../modules/user-access/services/AuthenticationService";

/* Infrastructure services */
export class HttpClient {
    public static async post<TResult>(resource: string, body: string): Promise<TResult> {
        const headers = await HttpClient.GetHeaders();
        const requestOptions: RequestInit = {
            method: 'POST',
            body: body,
            redirect: 'follow',
            headers: headers
        };

        const url = ApplicationSettings.API_URL + resource;
        const response = await fetch(url, requestOptions);
        
        if (response.status === 200) {
            return Promise.resolve(response as any);
        }
        else {
            return Promise.reject(new Error(response.statusText));
        }
    }

    public static async postForm<TResult>(resource: string, body: FormData): Promise<TResult> {
        const headers = await HttpClient.GetHeaders(null);
        const requestOptions: RequestInit = {
            method: 'POST',
            body: body,
            redirect: 'follow',
            headers: headers
        };

        const url = ApplicationSettings.API_URL + resource;
        const response = await fetch(url, requestOptions);
        
        if (response.status === 200) {
            return Promise.resolve(response.json());
        }
        else {
            return Promise.reject(new Error(response.statusText));
        }
    }

    public static async patch<TResult>(resource: string, body: string | null): Promise<TResult> {
        const headers = await HttpClient.GetHeaders();
        const requestOptions: RequestInit = {
            method: 'PATCH',
            body: body,
            redirect: 'follow',
            headers: headers
        };

        const url = ApplicationSettings.API_URL + resource;
        const response = await fetch(url, requestOptions);
        
        if (response.status === 200) {
            if (response.bodyUsed) {
                return Promise.resolve(response.json());
            }
            else {
                return response as any;
            }
        }
        else {
            return Promise.reject(new Error(response.statusText));
        }
    }

    public static async get<TResult>(resource: string): Promise<TResult> {
        const headers = await HttpClient.GetHeaders();
        const requestOptions: RequestInit = {
            method: 'GET',
            redirect: 'follow',
            headers: headers
        };

        const url = ApplicationSettings.API_URL + resource;
        const response = await fetch(url, requestOptions);
        
        if (response.status === 200) {
            return Promise.resolve(response.json());
        }

        if (response.status === 204) {
            return Promise.resolve(response.json());
        }
        else {
            return Promise.reject(new Error(response.statusText));
        }
    }

    private static async GetHeaders(contentType: null | string = 'application/json'): Promise<Record<string, string>> {
        const token = await getAccessToken();

        let headers: Record<string, string> = {};

        if (token != null) {
            headers['Authorization'] = 'Bearer ' + token;
        }

        if (contentType != null) {
            headers['Content-Type'] = contentType;
        }

        return headers;
    }
}
