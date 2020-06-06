﻿using Discord;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Skuld.Core.Utilities
{
    public static class HttpWebClient
    {
        public static string UAGENT = null;

        public static void SetUserAgent(ISelfUser self)
        {
            StringBuilder userAgent = new StringBuilder();

            userAgent.Append(self.Username);
            userAgent.Append("-");
            userAgent.Append(self.Discriminator);
            userAgent.Append("/");
            userAgent.Append(
                Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version
                .ToString()
            );
            userAgent.Append(" (Discord.Net; +");
            userAgent.Append(SkuldAppContext.Website);
            userAgent.Append(") DBots/");
            userAgent.Append(self.Id);

            UAGENT = userAgent.ToString();
        }

        public static HttpWebRequest CreateWebRequest(Uri uri, byte[] auth = null)
        {
            if(UAGENT == null)
            {
                throw new NullReferenceException(
                $"User Agent is not set, call {nameof(SetUserAgent)} method"
                );
            }
            var returncli = (HttpWebRequest)WebRequest.Create(uri);
            if (auth != null)
            {
                returncli.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(auth));
            }
            returncli.UserAgent = UAGENT;
            returncli.AllowAutoRedirect = true;
            returncli.KeepAlive = false;
            returncli.Timeout = 20000;
            returncli.ProtocolVersion = HttpVersion.Version11;

            return returncli;
        }

        #region Get

        public static async Task<string> ReturnStringAsync(Uri url, byte[] headers = null)
        {
            try
            {
                var client = CreateWebRequest(url, headers);

                var resp = (HttpWebResponse)(await client.GetResponseAsync().ConfigureAwait(false));
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    using var response = new StreamReader(resp.GetResponseStream());
                    var stringifiedresponse = await response.ReadToEndAsync().ConfigureAwait(false);
                    resp.Dispose();
                    client.Abort();
                    return stringifiedresponse;
                }
                else
                {
                    resp.Dispose();
                    client.Abort();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<byte[]> ReturnByteArrayAsync(Uri url, byte[] headers = null)
        {
            try
            {
                var client = CreateWebRequest(url, headers);

                var resp = (HttpWebResponse)(await client.GetResponseAsync().ConfigureAwait(false));
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    using var response = new MemoryStream();
                    await resp.GetResponseStream().CopyToAsync(response).ConfigureAwait(false);
                    resp.Dispose();
                    client.Abort();
                    response.Position = 0;
                    return response.ToArray();
                }
                else
                {
                    resp.Dispose();
                    client.Abort();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Stream> ReturnStreamAsync(Uri url, byte[] headers = null)
        {
            try
            {
                var client = CreateWebRequest(url, headers);

                var resp = (HttpWebResponse)(await client.GetResponseAsync().ConfigureAwait(false));
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var response = new MemoryStream();
                    await resp.GetResponseStream().CopyToAsync(response).ConfigureAwait(false);
                    resp.Dispose();
                    client.Abort();
                    response.Position = 0;
                    return response;
                }
                else
                {
                    resp.Dispose();
                    client.Abort();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<(HtmlDocument, Uri)> ScrapeUrlAsync(Uri url)
        {
            try
            {
                var request = CreateWebRequest(url);
                request.Timeout = 2000;
                request.UserAgent = UAGENT;

                try
                {
                    var response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                    var doc = new HtmlDocument();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        doc.Load(response.GetResponseStream(), Encoding.UTF8);
                        request.Abort();
                    }
                    if (doc != null)
                        return (doc, response.ResponseUri);
                    else
                        return (null, response.ResponseUri);
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        return (null, null);
                    }
                    throw;
                }
                catch
                {
                    return (null, null);
                }
            }
            catch
            {
                return (null, null);
            }
        }

        public static async Task<string> DownloadFileAsync(Uri url, string filepath)
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", UAGENT);
            await client.DownloadFileTaskAsync(url, filepath).ConfigureAwait(false);
            client.Dispose();
            return filepath;
        }

        #endregion Get

        #region Post

        public static async Task<string> PostStringAsync(Uri url, HttpContent content)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UAGENT);
            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");

            using HttpResponseMessage resp = await client.PostAsync(url, content).ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }

        public static async Task<Stream> PostStreamAsync(Uri url, HttpContent content)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UAGENT);
            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");

            using HttpResponseMessage resp = await client.PostAsync(url, content);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadAsStreamAsync();
            }
            else
            {
                return null;
            }
        }

        public static async Task<byte[]> PostByteArrayAsync(Uri url, HttpContent content)
        {
            using HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("User-Agent", UAGENT);
            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");

            using HttpResponseMessage resp = await client.PostAsync(url, content);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadAsByteArrayAsync();
            }
            else
            {
                return null;
            }
        }

        #endregion Post
    }
}