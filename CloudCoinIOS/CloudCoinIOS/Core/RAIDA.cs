using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CloudCoin_SafeScan
{
    public partial class RAIDA
    {

        public event EchoStatusChangedEventHandler EchoStatusChanged;
        public event DetectCoinCompletedEventHandler DetectCoinCompleted;
        public event StackScanCompletedEventHandler StackScanCompleted;

        public const short NODEQNTY = 25;
        public const short MINTRUSTEDNODES4AUTH = 8;
        public Node[] NodesArray = new Node[NODEQNTY];
        public enum Countries
        {
            Australia,
            Macedonia,
            Philippines,
            Serbia,
            Bulgaria,
            Russia3,
            Ukraine,
            UK,
            Punjab,
            Banglore,
            Texas,
            USA1,
            USA2,
            USA3,
            Romania,
            Taiwan,
            Russia1,
            Russia2,
            Columbia,
            Singapore,
            Germany,
            Canada,
            Venezuela,
            Hyperbad,
            Switzerland,
            Luxenburg
        };
        private static RAIDA theOnlyRAIDAInstance = new RAIDA();
        public static RAIDA Instance
        {
            get
            {
                return theOnlyRAIDAInstance;
            }
        }
        public EchoResponse[] EchoStatus = new EchoResponse[NODEQNTY];


        private RAIDA()
        {
            for (int i = 0; i < NodesArray.Length; i++)
            {
                NodesArray[i] = new Node(i);
            }
        }

        public void onEchoStatusChanged(EchoStatusChangedEventArgs e)
        {
            EchoStatusChanged?.Invoke(this, e);
        }

        public void getEcho()
        {
            Task<EchoResponse>[] tasks = new Task<EchoResponse>[NODEQNTY];
            int i = 0;
            foreach (Node node in Instance.NodesArray)
            {
                int index = i;
                tasks[i] = node.Echo();
                tasks[i].ContinueWith((anc) =>
                {
                    EchoStatus[index] = anc.Result;
                    //MessagingCenter.Send<RAIDA, int>(this, "EchoStatusChanged", index);
                    onEchoStatusChanged(new EchoStatusChangedEventArgs(index));
                });
                i++;
            }
            Task.Factory.ContinueWhenAll(tasks, ancestors =>
            {
                //MessagingCenter.Send<RAIDA, int>(this, "EchoStatusChanged", 25));
                onEchoStatusChanged(new EchoStatusChangedEventArgs(25));
            });
        }
        
        public async Task Detect(CloudCoin coin)
        {
            Stopwatch sw = new Stopwatch();
            Task<DetectResponse>[] tasks = new Task<DetectResponse>[NODEQNTY];
            sw.Start();
            foreach(Node node in Instance.NodesArray)
            {
                tasks[node.Number] = node.Detect(coin);
                var tmp = await tasks[node.Number];
                coin.detectStatus[node.Number] = (tmp.status == "pass") ? CloudCoin.raidaNodeResponse.pass : (tmp.status == "fail") ? CloudCoin.raidaNodeResponse.fail : CloudCoin.raidaNodeResponse.error;
            }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Factory.ContinueWhenAll(tasks, (ancs) => 
            {
                sw.Stop();
                onDetectCoinCompleted(new DetectCoinCompletedEventArgs(coin, sw));
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public void onDetectCoinCompleted(DetectCoinCompletedEventArgs e)
        {
            DetectCoinCompleted?.Invoke(this, e);
        }

        public void onStackScanCompleted(StackScanCompletedEventArgs e)
        {
            StackScanCompleted?.Invoke(this, e);
        }

        public void Detect(CoinStack stack, bool isCoinToBeImported)
        {
            Stopwatch total = new Stopwatch();
            total.Start();
            Task[] checkStackTasks = new Task[stack.cloudcoin.Count()];
            Stopwatch[] tw = new Stopwatch[stack.cloudcoin.Count()];
            int k = 0;
            foreach(CloudCoin coin in stack)    // int k = 0; k < stack.cloudcoin.Count(); k++)
            {
                //var coin = stack.cloudcoin[k];
                if (!isCoinToBeImported)
                {
                    coin.pans = coin.an;
                }
                Task<DetectResponse>[] checkCoinTasks = new Task<DetectResponse>[NODEQNTY];
                var t = tw[k] = new Stopwatch();
                t.Start();
                foreach (Node node in Instance.NodesArray)
                {
                    checkCoinTasks[node.Number] = node.Detect(coin);
                    checkCoinTasks[node.Number].ContinueWith((anc) =>
                    {
                        var tmp = anc.Result;
                        coin.detectStatus[node.Number] = (tmp.status == "pass") ? CloudCoin.raidaNodeResponse.pass : (tmp.status == "fail") ? CloudCoin.raidaNodeResponse.fail : CloudCoin.raidaNodeResponse.error;
                    });
                }
                checkStackTasks[k] = Task.Factory.ContinueWhenAll(checkCoinTasks, (ancs) => 
                {
                    t.Stop();
                    onDetectCoinCompleted(new DetectCoinCompletedEventArgs(coin, t));
                });
                k++;
            }
            Task.Factory.ContinueWhenAll(checkStackTasks, (ancs) =>
            {
                onStackScanCompleted(new StackScanCompletedEventArgs(stack, total));
            });
        }
                
        public partial class Node
        {
            public int Number;
            public Countries Country
            {
                get
                {
                    switch (Number)
                    {
                        case 0: return Countries.Australia;
                        case 1: return Countries.Macedonia;
                        case 2: return Countries.Philippines;
                        case 3: return Countries.Serbia;
                        case 4: return Countries.Bulgaria;
                        case 5: return Countries.Russia3;
                        case 6: return Countries.Switzerland;
                        case 7: return Countries.UK;
                        case 8: return Countries.Punjab;
                        case 9: return Countries.Banglore;
                        case 10: return Countries.Texas;
                        case 11: return Countries.USA1;
                        case 12: return Countries.Romania;
                        case 13: return Countries.Taiwan;
                        case 14: return Countries.Russia1;
                        case 15: return Countries.Russia2;
                        case 16: return Countries.Columbia;
                        case 17: return Countries.Singapore;
                        case 18: return Countries.Germany;
                        case 19: return Countries.Canada;
                        case 20: return Countries.Venezuela;
                        case 21: return Countries.Hyperbad;
                        case 22: return Countries.USA2;
                        case 23: return Countries.Ukraine;
                        case 24: return Countries.Luxenburg;
                        default: return Countries.USA3;
                    }
                }
            }

            public string Name { get; set; }
            public Uri BaseUri
            {
                get { return new Uri("https://RAIDA" + Number.ToString() + ".cloudcoin.global/service"); }
            }
            public Uri BackupUri
            {
                get { return new Uri("https://RAIDA" + Number.ToString() + ".cloudcoin.ch/service"); }
            }
            public EchoResponse LastEchoStatus;
            public DetectResponse LastDetectResult;
            public string Location
            {
                get
                {
                    return Country.ToString();
                }
            }

            public Node(int number)
            {
                Number = number;
                Name = "RAIDA" + number.ToString();
            }

            public async Task<EchoResponse> Echo()
            {
                var client = new RestClient();
                //client.BaseUrl = BaseUri;
                var request = new RestRequest("echo");
                client.BaseUrl = BaseUri;
                request.Timeout = 2000;
                EchoResponse get_Echo = new EchoResponse();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
					get_Echo = JsonConvert.DeserializeObject<EchoResponse>(client.Execute(request).Content);
                }
                catch (JsonException e)
                {
                    get_Echo = new EchoResponse(Name, "Invalid respose", e.Message, DateTime.Now.ToString());

                }
                get_Echo = get_Echo ?? new EchoResponse(Name, "Network problem", "Node not found", DateTime.Now.ToString());
                //if (getEcho. != null)
                //    getEcho = new EchoResponse(Name, "Network problem", getEcho.ErrorMessage, DateTime.Now.ToString());

                sw.Stop();
                get_Echo.responseTime = sw.Elapsed;
                LastEchoStatus = get_Echo;

                return get_Echo;
            }

			public async Task<string> GetJSON(string url, bool isArray = true, int attempt = 0)
			{
				var client = new HttpClient();
				client.MaxResponseContentBufferSize = 256000;

				client.Timeout = TimeSpan.FromSeconds(20);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var cts = new CancellationTokenSource();

				try
				{
					//cts.CancelAfter(2 * 1000  ); //ms
					var response = await client.GetAsync(url, cts.Token); // must NOT use leading slash in path
					if (!response.IsSuccessStatusCode)
						throw new HttpRequestException();
					string jsonResult = await response.Content.ReadAsStringAsync();
					return jsonResult;
				}
				catch (Exception e)
				{
					if (attempt < 4)
					{
						System.Diagnostics.Debug.WriteLine("FAILURE, RETRY", attempt + 1);
						return null;// GetJSONArray(path, isArray, attempt += 1);
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("MULTIPLE FAIL, QUIT");
						return null;
					}
				}
			}
            
            public async Task<DetectResponse> Detect(CloudCoin coin)
            {
				var getDetectResult = new DetectResponse();
				var sw = new Stopwatch();
				sw.Start();
				var query = string.Format("/detect?nn={0}&sn={1}&an={2}&pan={3}&denomination={4}",
				                          coin.nn.ToString(), coin.sn.ToString(), 
				                          coin.an[Number], coin.pans[Number],
										  Utils.Denomination2Int(coin.denomination).ToString());
				var detectResult = await GetJSON(BaseUri + query);

                try
                {
                    getDetectResult = JsonConvert.DeserializeObject<DetectResponse>(detectResult);
                }
                catch (JsonException e)
                {
                    getDetectResult = new DetectResponse(Name, coin.sn.ToString(), "Invalid response", "The server does not respond or returns invalid data", DateTime.Now.ToString());
					Console.WriteLine(e.Message);
                }
                
				getDetectResult = getDetectResult ?? new DetectResponse(Name, coin.sn.ToString(), "Network problem", "Node not found", DateTime.Now.ToString());
                
                sw.Stop();
                getDetectResult.responseTime = sw.Elapsed;

				if (getDetectResult.status == "pass")
                {
                    coin.an[Number] = coin.pans[Number];
                }

                switch (getDetectResult.status)
                {
                    case "pass":
                        coin.detectStatus[Number] = CloudCoin.raidaNodeResponse.pass;
                        break;
                    case "fail":
                        coin.detectStatus[Number] = CloudCoin.raidaNodeResponse.fail;
                        break;
                    default:
                        coin.detectStatus[Number] = CloudCoin.raidaNodeResponse.error;
                        break;
                }

                return getDetectResult;
            }
                        
            public override string ToString()
            {
                string result = "Server: " + Name +
                    "\nLocation: " + Location +
                    "\nStatus: " + LastEchoStatus.status +
                    "\nEcho: " + LastEchoStatus.responseTime.ToString("sfff") + "ms";
                return result;
            }

            public string ToString(DetectResponse res)
            {
                string result = "Server: " + Number +
                    "\nLocation: " + Location +
                    "\nStatus: " + res.status +
                    "\nEcho: " + res.responseTime.ToString("sfff") + "ms";
                return result;
            }
        }

        public class EchoResponse //: RestResponse<EchoResponse>
        {
            public string server { get; set; }
            public string status { get; set; }
            public string message { get; set; }
            public string time { get; set; }
            public TimeSpan responseTime { get; set; }

            public EchoResponse()
            {

                server = "unknown";
                status = "unknown";
                message = "empty";
                time = "";
            }

            public EchoResponse(string server, string status, string message, string time)
            {
                this.server = server;
                this.status = status;
                this.message = message;
                this.time = time;
            }
        }

        public class DetectResponse //: RestResponse<DetectResponse>
        {
            public string server { get; set; }
            public string status { get; set; }
            public string sn { get; set; }
            public string message { get; set; }
            public string time { get; set; }
            public TimeSpan responseTime { get; set; }

            public DetectResponse()
            {

                server = "unknown";
                status = "unknown";
                message = "empty";
                time = "";
            }

            public DetectResponse(string server, string sn, string status, string message, string time)
            {
                this.server = server;
                this.sn = sn;
                this.status = status;
                this.message = message;
                this.time = time;
            }
        }
    }
}
