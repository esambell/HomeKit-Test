
/*
 * Minimal Apple Homekit emulator with minimal dependencies
 * (c) Eric Sambell 2020 MIT License
 * 
 * SHA-512 based on Crhis Veness Java Script SHA-512 implementation (https://www.movable-type.co.uk/scripts/sha512.html)
 * Big integare arithmatic based on answer from: https://stackoverflow.com/questions/2207006/modular-exponentiation-for-high-numbers-in-c by clinux
 * Modular inverse calculation from https://www.di-mgt.com.au/euclidean.html#code-modinv implementation of KNU298 avoiding negative integers
 * Chacha20 from https://github.com/sbennett1990/ChaCha20-csharp
 * ed25519 primarily from RFC 8032, https://github.com/hanswolff/ed25519 used for guidance
 * 
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Makaretu.Dns;
using System.Numerics;
using System.Security.Policy;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Net.Configuration;

namespace HomeKit_Test
{
    public partial class Form1 : Form
    {

        struct UInt64c
        {
            public UInt32 lo;
            public UInt32 hi;
        };
        struct int32Array
        {
            public UInt32[] digits;
            public Boolean negative;
        }

        String[] k =
            { "428a2f98d728ae22", "7137449123ef65cd", "b5c0fbcfec4d3b2f", "e9b5dba58189dbbc",
            "3956c25bf348b538", "59f111f1b605d019", "923f82a4af194f9b", "ab1c5ed5da6d8118",
            "d807aa98a3030242", "12835b0145706fbe", "243185be4ee4b28c", "550c7dc3d5ffb4e2",
            "72be5d74f27b896f", "80deb1fe3b1696b1", "9bdc06a725c71235", "c19bf174cf692694",
            "e49b69c19ef14ad2", "efbe4786384f25e3", "0fc19dc68b8cd5b5", "240ca1cc77ac9c65",
            "2de92c6f592b0275", "4a7484aa6ea6e483", "5cb0a9dcbd41fbd4", "76f988da831153b5",
            "983e5152ee66dfab", "a831c66d2db43210", "b00327c898fb213f", "bf597fc7beef0ee4",
            "c6e00bf33da88fc2", "d5a79147930aa725", "06ca6351e003826f", "142929670a0e6e70",
            "27b70a8546d22ffc", "2e1b21385c26c926", "4d2c6dfc5ac42aed", "53380d139d95b3df",
            "650a73548baf63de", "766a0abb3c77b2a8", "81c2c92e47edaee6", "92722c851482353b",
            "a2bfe8a14cf10364", "a81a664bbc423001", "c24b8b70d0f89791", "c76c51a30654be30",
            "d192e819d6ef5218", "d69906245565a910", "f40e35855771202a", "106aa07032bbd1b8",
            "19a4c116b8d2d0c8", "1e376c085141ab53", "2748774cdf8eeb99", "34b0bcb5e19b48a8",
            "391c0cb3c5c95a63", "4ed8aa4ae3418acb", "5b9cca4f7763e373", "682e6ff3d6b2b8a3",
            "748f82ee5defb2fc", "78a5636f43172f60", "84c87814a1f0ab72", "8cc702081a6439ec",
            "90befffa23631e28", "a4506cebde82bde9", "bef9a3f7b2c67915", "c67178f2e372532b",
            "ca273eceea26619c", "d186b8c721c0c207", "eada7dd6cde0eb1e", "f57d4f7fee6ed178",
            "06f067aa72176fba", "0a637dc5a2c898a6", "113f9804bef90dae", "1b710b35131c471b",
            "28db77f523047d84", "32caab7b40c72493", "3c9ebe0a15c9bebc", "431d67c49c100d4c",
            "4cc5d4becb3e42b6", "597f299cfc657e2a", "5fcb6fab3ad6faec", "6c44198c4a475817"};
        UInt64c[] K = new UInt64c[80];


        String[] h =
            {  "6a09e667f3bcc908", "bb67ae8584caa73b", "3c6ef372fe94f82b", "a54ff53a5f1d36f1",
            "510e527fade682d1", "9b05688c2b3e6c1f", "1f83d9abfb41bd6b", "5be0cd19137e2179"};
        UInt64c[] HInitial = new UInt64c[80];

        UInt32[] N = {  0xFFFFFFFF, 0xFFFFFFFF, 0xC90FDAA2, 0x2168C234, 0xC4C6628B ,0x80DC1CD1, 0x29024E08, 0x8A67CC74,
                        0x020BBEA6, 0x3B139B22, 0x514A0879, 0x8E3404DD, 0xEF9519B3, 0xCD3A431B, 0x302B0A6D, 0xF25F1437,
                        0x4FE1356D, 0x6D51C245, 0xE485B576, 0x625E7EC6, 0xF44C42E9, 0xA637ED6B, 0x0BFF5CB6, 0xF406B7ED,
                        0xEE386BFB, 0x5A899FA5, 0xAE9F2411, 0x7C4B1FE6, 0x49286651, 0xECE45B3D, 0xC2007CB8, 0xA163BF05,
                        0x98DA4836, 0x1C55D39A, 0x69163FA8, 0xFD24CF5F, 0x83655D23, 0xDCA3AD96, 0x1C62F356, 0x208552BB,
                        0x9ED52907, 0x7096966D, 0x670C354E, 0x4ABC9804, 0xF1746C08, 0xCA18217C, 0x32905E46, 0x2E36CE3B,
                        0xE39E772C, 0x180E8603, 0x9B2783A2, 0xEC07A28F, 0xB5C55DF0, 0x6F4C52C9, 0xDE2BCBF6, 0x95581718,
                        0x3995497C, 0xEA956AE5, 0x15D22618, 0x98FA0510, 0x15728E5A, 0x8AAAC42D, 0xAD33170D, 0x04507A33,
                        0xA85521AB, 0xDF1CBA64, 0xECFB8504, 0x58DBEF0A, 0x8AEA7157, 0x5D060C7D, 0xB3970F85, 0xA6E1E4C7,
                        0xABF5AE8C, 0xDB0933D7, 0x1E8C94E0, 0x4A25619D, 0xCEE3D226, 0x1AD2EE6B, 0xF12FFA06, 0xD98A0864,
                        0xD8760273, 0x3EC86A64, 0x521F2B18, 0x177B200C, 0xBBE11757, 0x7A615D6C, 0x770988C0, 0xBAD946E2,
                        0x08E24FA0, 0x74E5AB31, 0x43DB5BFC, 0xE0FD108E, 0x4B82D120, 0xA93AD2CA, 0xFFFFFFFF, 0xFFFFFFFF
        };

        UInt32[] s = { 0xBEB25379, 0xD1A8581E, 0xB5A72767, 0x3A2441EE };

        UInt32[] a = {  0x60975527, 0x035CF2AD, 0x1989806F, 0x0407210B,
                        0xC81EDC04, 0xE2762A56, 0xAFD529DD, 0xDA2D4393
        };

        UInt32[] b = {  0xE487CB59, 0xD31AC550, 0x471E81F0, 0x0F6928E0,
            0x1DDA08E9, 0x74A004F4, 0x9E61F5D1, 0x05284D20
        };

        UInt32[] storedk;
        UInt32[] storedB;
        UInt32[] storedA;
        UInt32[] storedv;
        UInt32[] storedb;
        UInt32[] storedK;
        UInt32[] storedx;
        UInt32[] storedu;
        UInt32[] storedS;
        Byte[] storeds;

        const string cfgFile  = "hap.cfg";
        const byte cfgDeviceLTPK = 0x00;
        const byte cfgDevicePairingID = 0x01;
        const byte cfgAccessoryLTSK = 0x02;

        Byte[] dataDeviceLTPK = new byte[0];
        Byte[] dataDevicePairingID = new byte[0];
        Byte[] dataAccessoryLTSK = new byte[] {0x9d, 0x61, 0xb1, 0x9d, 0xef, 0xfd, 0x5a, 0x60, 0xba, 0x84, 0x4a, 0xf4, 0x92, 0xec, 0x2c, 0xc4,
                                        0x44, 0x49, 0xc5, 0x69, 0x7b, 0x32, 0x69, 0x19, 0x70, 0x3b, 0xac, 0x03, 0x1c, 0xae, 0x7f, 0x60};
        string dataPairingID = "12:13:12:12:12:13";


        const int DevicePort = 5252;
        const string DeviceCode = "143-83-105";
        const Byte generator = 5;
        UInt32[] g = { generator };
        Random random = new Random();
        String userName = "Pair-Setup";
        MulticastService mdns;

        bool paired = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            K = initHexArray(k);
            HInitial = initHexArray(h);
            N = UInt32ArrayReverse(N);
            a = UInt32ArrayReverse(a);
            b = UInt32ArrayReverse(b);
            s = UInt32ArrayReverse(s);

            label2.Text = DeviceCode;

            mdns = new MulticastService(NetFilter);
            mdns.UseIpv4 = true;
            mdns.UseIpv6 = false;

            var sd = new ServiceDiscovery(mdns);

            var za = new ServiceProfile("Test HAP", "_hap._tcp", DevicePort);
            za.AddProperty("c#", "12");
            za.AddProperty("ff", "0");
            za.AddProperty("id", dataPairingID);
            za.AddProperty("md", "Ver 1");
            za.AddProperty("sf", "1");
            za.AddProperty("ci", "1");
            za.AddProperty("pv", "1.1");
            sd.Advertise(za);

            mdns.Start();
            TCPListenerTask.RunWorkerAsync();

            init_ed25519();

            int32Array A, B, mod;
            A.digits = new uint[] { 0x06, 0xFFFFFFFF, 0xFFFF };
            A.digits = new uint[] { 0x5 };

            A.negative = false;
            BigInteger bigA = new BigInteger(fromUInt32ArrayLE(A.digits).Concat(new Byte[] { 0 }).ToArray());

            B.digits = new uint[] { 0xFFFFF };
            ////b.digits = s.Concat(new UInt32[] {  0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 
            //                                    0x12345678, 0x12345678, 0x12345678, 0x12345678

            //}).ToArray();
            B.digits = new UInt32[] {   0xd1ff15eb , 0x16a1e0f1 , 0x0fb9ceaf , 0xb2b1db62 , 0x7b288f80 , 0xeb07c3ba , 0x5c182e5d , 0x141534d5,
                                        0x6693bac4 , 0xdccda4c6 , 0x577a8179 , 0xe520c965 , 0xa4907a1a , 0x81c38c8b , 0x544fa6e3 , 0xf36091f1
            };

            //b.digits = new UInt32[] {   0xd1ff15eb , 0x16a1e0f1 , 0x0fb9ceaf , 0xb2b1db62 , 0x7b288f80 , 0xeb07c3ba , 0x5c182e5d , 0x141534d5,
            //                            0x6693bac4 , 0xdccda4c6 , 0x577a8179 , 0xe520c965 , 0xa4907a1a , 0x81c38c8b , 0x8FFFFFFF
            //};

            //b.digits = new uint[] { 0x8FFFFFF, 0x8FFFFFFF, 0x8FFFFFFF };

            B.digits = a;
            B.negative = false;
            BigInteger bigB = new BigInteger(fromUInt32ArrayLE(B.digits).Concat(new Byte[] { 0 }).ToArray());

            mod.digits = new uint[] { 0xFFFFFFFF, 0xFFFFF };
            mod.digits = N;

            //mod.digits = new uint[] { 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000

            //};
            mod.negative = false;
            BigInteger bigMod = new BigInteger(fromUInt32ArrayLE(mod.digits).Concat(new Byte[] { 0 }).ToArray());

            int32Array testResult;
            testResult.digits = new UInt32[] { 0 };
            //testResult.negative = false;
            // testResult = int32ArrayAddNoSign(a, b, mod);
            //testResult.digits = UInt32ArraySubSimple(a.digits, b.digits);
            //testResult.digits = UInt32ArrayMulNoSign(a.digits, b.digits, mod.digits);
            //testResult.digits = UInt32ArrayMod(a.digits, mod.digits);
            //testResult = int32ArraySHR(a);
            //testResult.digits = UInt32PowMod(A.digits, b.digits, mod.digits);


            UInt32[] q;

            //UInt32ArrayDiv(new uint[] { 0xFFFFFFF2, 0XFFF}, new uint[] { 0xF2342342, 0x12}, out q, out testResult.digits) ;

            //UInt32[] m = new uint[] { 7 };
            //UInt32[] R = new uint[] { 0x0,0x0, 0x1 };
            //UInt32[] ba = new uint[] { 0x0, 0x1 };

            //UInt32[] R_inv = UInt32ArrayGetInverse(R, m);

            //UInt32[] m_inv = UInt32ArrayGetInverse(m, ba);
            //UInt32[] m_prime = UInt32ArraySubSimple(ba, m_inv);
            //UInt32ArrayShrink(ref m_prime);

            UInt32[] temp = new uint[] { 0xffffffff, 0xf };
            UInt32[] temp1 = new uint[] { 0xfffffff };

            Byte[] secret = new byte[] {0x9d, 0x61, 0xb1, 0x9d, 0xef, 0xfd, 0x5a, 0x60, 0xba, 0x84, 0x4a, 0xf4, 0x92, 0xec, 0x2c, 0xc4,
                                        0x44, 0x49, 0xc5, 0x69, 0x7b, 0x32, 0x69, 0x19, 0x70, 0x3b, 0xac, 0x03, 0x1c, 0xae, 0x7f, 0x60};

            secret = new byte[] {0x4c, 0xcd, 0x08, 0x9b, 0x28, 0xff, 0x96, 0xda, 0x9d, 0xb6, 0xc3, 0x46, 0xec, 0x11, 0x4e, 0x0f,
                                        0x5b, 0x8a, 0x31, 0x9f, 0x35, 0xab, 0xa6, 0x24, 0xda, 0x8c, 0xf6, 0xed, 0x4f, 0xb8, 0xa6, 0xfb};

            Byte[] msg = new byte[0];

            msg = new byte[] { 0x72 };

            //Byte[] publicKey = ed25519PublicKey(secret);

            //Byte[] signature = ed25519sign(secret, msg);

            //signature[22] = 0x69;

            //msg = new byte[] { 0x71 };

            //publicKey[12] = 0x24;


            //bool result = ed25519verify(publicKey, msg, signature);

            //addBigIntToLogBox(ed25519G.X);
            //addBigIntToLogBox(ed25519G.Y);
            //addBigIntToLogBox(ed25519G.Z);
            //addBigIntToLogBox(ed25519G.T);


            //UInt32[] inv = ed25519modInv(new uint[] { 0x1db42 });
            //UInt32[] d = UInt32ArrayMulNoSign(new uint[] { 0x1db41 }, inv);
            //d = UInt32ArrayMod(d, ed25519p);
            //d = UInt32ArraySubSimple(ed25519p, d);

            //UInt32ArraySHRWords(ref temp, 25);

            //testResult.digits = UInt32ArrayMulNoSign(temp, temp1);

            //foreach (UInt32 i in signature)
            //{
            //    //AddToLogBox(i.ToString() + " " + i.ToString("X8") + "\r\n");
            //    AddToLogBox(i.ToString("x2"));
            //}
            //AddToLogBox("\r\n");
            //AddToLogBox(result.ToString() + "\r\n");


            //Byte[] byteTest = new byte[] { 0x69, 0x69 };


            //appendTLVBytes(ref byteTest, 0x12, Enumerable.Repeat((Byte)0x61,300).ToArray());

            //foreach (Byte i in byteTest)
            //{
            //    //AddToLogBox(i.ToString() + " " + i.ToString("X8") + "\r\n");
            //    AddToLogBox(i.ToString("X2") + "\r\n");
            //}

            //       BigInteger bigIntTest = BigInteger.ModPow(bigA, bigB, bigMod);
            //   AddToLogBox(bigIntTest.ToString("X8") + "\r\n");

            //int testCmp = int32ArrayCmp(A, B);
            //AddToLogBox(testCmp.ToString() + "\r\n");

        }
        private IEnumerable<System.Net.NetworkInformation.NetworkInterface> NetFilter(IEnumerable<System.Net.NetworkInformation.NetworkInterface> InterfacesIn)
        {

            IEnumerable<System.Net.NetworkInformation.NetworkInterface> InterfacesOut;

            //foreach(NetworkInterface CurNetworkInterface in InterfacesIn)
            //{
            //    MessageBox.Show(CurNetworkInterface.Name);
            //}

            InterfacesOut = InterfacesIn.Where(x => x.Name == "Ethernet 4");

            //foreach (NetworkInterface CurNetworkInterface in InterfacesOut)
            //{
            //    MessageBox.Show(CurNetworkInterface.Name);
            //}

            return InterfacesOut;
        }

        private void TCPListenerTask_DoWork(object sender, DoWorkEventArgs e)
        {
            TcpListener server = new TcpListener(IPAddress.Any, DevicePort);
            server.Start();
            Byte[] bytes = new Byte[0xfFff];
            String data = null;
            int numConnects = 0;
            Byte[] received = new Byte[1];

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                data = null;

                int i;
                numConnects++;
                label1.BeginInvoke((MethodInvoker)delegate ()
                {
                    label1.Text = numConnects.ToString();
                });
                NetworkStream stream = client.GetStream();
                String request = null;
                bool isLineBlank = true;
                bool parsingHeader = true;
                int dataBytesReceived = 0;
                int contentLength = 0;
                string requestType = null;
                string uri = null;
                Byte curMethod = 0;
                

                    while ((i = stream.Read(received, 0, 1)) != 0)
                    {

                        if (parsingHeader)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(received, 0, i);
                            request += data;

                            if (data == "\n" && isLineBlank)
                            {
                                AddToLogBox(request + "End" + "\r\n");

                                if (request.Substring(0, 4) == "POST")
                                {
                                    requestType = "POST";
                                }
                                int slashIndex = request.IndexOf('/');
                                int spaceIndex = request.IndexOf(' ', slashIndex);
                                uri = request.Substring(slashIndex, spaceIndex - slashIndex);
                                int contentLengthIndex;

                                if ((contentLengthIndex = request.IndexOf("Content-Length:") + 15) != 15)
                                {
                                    int lineEndIndex = request.IndexOf('\r', contentLengthIndex);
                                    contentLength = int.Parse(request.Substring(contentLengthIndex + 1, lineEndIndex - contentLengthIndex - 1));
                                }
                                else contentLength = 0;

                                AddToLogBox(requestType + "\r\n");
                                AddToLogBox(uri + "\r\n");
                                AddToLogBox(contentLength.ToString() + "\r\n");

                                parsingHeader = false;
                            }
                            if (data == "\n")
                            {
                                isLineBlank = true;
                            }
                            else if (data != "\r")
                            {
                                isLineBlank = false;
                            }
                        }
                        else if (!parsingHeader)
                        {
                            bytes[dataBytesReceived++] = received[0];

                        }

                        if (dataBytesReceived == contentLength && !parsingHeader)
                        {
                            AddToLogBox(dataBytesReceived.ToString() + " Bytes Recevied\r\n");
                            processHTTP(stream, requestType, uri, bytes, dataBytesReceived, ref curMethod);
                            parsingHeader = true;
                            request = "";
                            dataBytesReceived = 0;
                        }
                        

                    }

                client.Close();
                AddToLogBox("Conenction Closed\r\n");
               
            }



        }

        void processHTTP(NetworkStream stream, String requestType, String uri, Byte[] bytes, int dataBytesReceived, ref Byte curMethod)
        {
            switch (requestType)
            {
                case "POST":
                    switch (uri)
                    {
                        case "/pair-setup":
                       
                            Byte[] TLVState = findTLVKey(bytes, 0x06, dataBytesReceived);
                            if (TLVState == null || TLVState.Length == 0) break;
                            int state = TLVState[0];
                            AddToLogBox("State: " + state + "\r\n");
                            switch (state)
                            {
                                case 1:

                                    {
                                        if (paired)
                                        {
                                            Byte[] responseBytes = new Byte[] { 0x06, 0x01, 0x02, 0x07, 0x01, 0x06 };
                                            sendTLVResponse(stream, responseBytes);


                                        }
                                        else
                                        {
                                            

                                            storeds = byteGenRandom(16);

                                            storedx = generatePrivateKey(toUInt32Array(storeds), userName, DeviceCode);
                                            storedv = generateVerifier(storedx);
                                            storedk = generateMultiplier();
                                            //storedA = generatePublicA(a);
                                            storedb = toUInt32Array(byteGenRandom(32));
                                            storedB = generatePublicB(storedk, storedv, storedb);
                                            

                                            Byte[] TLVResponse = new byte[0];
                                            appendTLVBytes(ref TLVResponse, 0x06, new byte[] { 0x02 });
                                            appendTLVBytes(ref TLVResponse, 0x02, storeds);
                                            appendTLVBytes(ref TLVResponse, 0x03, fromUInt32Array(storedB));

                                            sendTLVResponse(stream, TLVResponse);

                                        }

                                        break;
                                    }
                                case 3:
                                    {
                                        Byte[] publicA = findTLVKey(bytes, 0x03, dataBytesReceived);
                                        Byte[] proofA = findTLVKey(bytes, 0x04, dataBytesReceived);

                                        UInt32[] publicAUInt32 = toUInt32Array(publicA);
                                        publicAUInt32 = UInt32ArrayMod(publicAUInt32, N);
                                        if (UInt32ArrayCmpNoSign(publicAUInt32, new UInt32[] { 0x0 }) == 0)
                                        {
                                            AddToLogBox("A mod N == 0\r\n");
                                            sendPairingError(stream, 0x04, 0x02);
                                            break;
                                        }

                                        storedu = generateScrambling(toUInt32Array(publicA), storedB);
                                        storedS = generateSessionSecret(toUInt32Array(publicA), storedv, storedu, storedb);
                                        storedK = generateSessionKey(storedS);

                                        Byte[] hashN = genSHA512(N);
                                        Byte[] gPad = new byte[1];
                                        gPad[gPad.Length - 1] = generator;
                                        Byte[] hashG = genSHA512(gPad);
                                        Byte[] NxorG = new Byte[hashN.Length];
                                        for (int i = 0; i < NxorG.Length;i++) NxorG[i]= (byte)(hashN[i] ^ hashG[i]);
                                        Byte[] hashI = genSHA512(userName);
                                        Byte[] publicB = fromUInt32Array(storedB);
                                        Byte[] K = fromUInt32Array(storedK);

                                        Byte[] localProofA = genSHA512(NxorG, hashI, storeds, publicA, publicB, K);


                                    
                                        AddToLogBox("public A Length: " + publicA.Length.ToString() + "\r\n");
                                        if (proofA == null)
                                        {
                                            AddToLogBox("proof A null");
                                            break;
                                        }
                                        AddToLogBox("proof A Length: " + proofA.Length.ToString() + "\r\n");
                                        AddToLogBox(byteArrayEqual(proofA, localProofA).ToString() + "\r\n");

                                        if (!byteArrayEqual(proofA, localProofA))                                        
                                        {
                                            AddToLogBox("Client Proof Mismatch\r\n");
                                            sendPairingError(stream, 0x04, 0x02);
                                            break;
                                        }

                                        byte[] proofB = genSHA512(publicA, proofA, K);
                                       
                                        if (checkBox1.Checked) proofB = byteArrayReverse(proofB);

                                        Byte[] TLVResponse = new byte[0];
                                        appendTLVBytes(ref TLVResponse, 0x06, new byte[] { 0x04 });
                                        appendTLVBytes(ref TLVResponse, 0x04, proofB);
                                        sendTLVResponse(stream, TLVResponse);

                                        AddToLogBox("Server Proof Sent\r\n");

                                        break;

                                    }
                                case 5:
                                    {
                                        Byte[] encData = findTLVKey(bytes, 0x05, dataBytesReceived);
                                        Byte[] chachaKey = genHKDFSHA512(fromUInt32Array(storedK), "Pair-Setup-Encrypt-Salt", "Pair-Setup-Encrypt-Info", 32);
                                        Byte[] nonce = Encoding.UTF8.GetBytes("PS-Msg05");
                                        
                                        Byte[] subTLV = new byte[encData.Length - 16];
                                        Byte[] authTag = new byte[16];

                                        for (int i = 0; i < subTLV.Length; i++) subTLV[i] = encData[i];
                                        for (int i = 0; i < 16; i++) authTag[i] = encData[encData.Length - 16 + i];

                                        if (verifyAuthTag(subTLV, authTag, chachaKey, nonce)) AddToLogBox("M5 decrypt successful\r\n");
                                        else
                                        {
                                            sendPairingError(stream, 0x06, 0x02);
                                            AddToLogBox("M5 Auth Tag Fail\r\n");
                                            break;
                                        }
                                       
                                        subTLV = chacha20(chachaKey, nonce, subTLV, 1);

                                        dataDevicePairingID = findTLVKey(subTLV, 0x01, subTLV.Length);
                                        dataDeviceLTPK = findTLVKey(subTLV, 0x03, subTLV.Length);
                                        Byte[] deviceSignature = findTLVKey(subTLV, 0x0a, subTLV.Length);

                                        Byte[] deviceX = genHKDFSHA512(fromUInt32Array(storedK), "Pair-Setup-Controller-Sign-Salt", "Pair-Setup-Controller-Sign-Info", 32);
                                        Byte[] deviceInfo = deviceX.Concat(dataDevicePairingID).Concat(dataDeviceLTPK).ToArray();

                                        AddToLogBox(Encoding.UTF8.GetString(dataDevicePairingID) + "\r\n");
                                        
                                        bool result = ed25519verify(dataDeviceLTPK, deviceInfo, deviceSignature);
                                        AddToLogBox(result.ToString() + "\r\n");

                                        writeCfg();

                                        Byte[] pairingID = Encoding.UTF8.GetBytes(dataPairingID);

                                        Byte[] accessoryLTPK = ed25519PublicKey(dataAccessoryLTSK);
                                        Byte[] accessoryX = genHKDFSHA512(fromUInt32Array(storedK), "Pair-Setup-Accessory-Sign-Salt", "Pair-Setup-Accessory-Sign-Info", 32);
                                        Byte[] accessoryInfo = accessoryX.Concat(pairingID).Concat(accessoryLTPK).ToArray();

                                        Byte[] accessorySignature = ed25519sign(dataAccessoryLTSK, accessoryInfo);

                                        subTLV = new byte[0];
                                        appendTLVBytes(ref subTLV, 0x01, pairingID);
                                        appendTLVBytes(ref subTLV, 0x03, accessoryLTPK);
                                        appendTLVBytes(ref subTLV, 0x0a, accessorySignature);

                                        nonce = Encoding.UTF8.GetBytes("PS-Msg06");

                                        encData = chacha20(chachaKey, nonce, subTLV, 1);
                                        authTag = genAuthTag(encData, chachaKey, nonce);
                                        Byte[] respData = encData.Concat(authTag).ToArray();

                                        Byte[] response = new byte[0];

                                        appendTLVBytes(ref response, 0x06, new byte[] { 0x06 });
                                        appendTLVBytes(ref response, 0x05, respData);

                                        sendTLVResponse(stream, response);


                                        AddToLogBox(verifyAuthTag(encData, authTag, chachaKey, nonce).ToString() + "\r\n");
                                        
                                        AddToLogBox("Reached Level 5\r\n");
                                        break;
                                    }

                            }
                            break;

                    }

                    break;
            }

        }

        void writeCfg()
        {
            Byte[] toWrite = new byte[0];

            toWrite = toWrite.Concat(genCfgItem(cfgDeviceLTPK, dataDeviceLTPK)).ToArray();
            toWrite = toWrite.Concat(genCfgItem(cfgDevicePairingID, dataDevicePairingID)).ToArray();
            toWrite = toWrite.Concat(genCfgItem(cfgAccessoryLTSK, dataAccessoryLTSK)).ToArray();

            File.WriteAllBytes(cfgFile, toWrite);
        }

        Byte[] genCfgItem(Byte cfgID, Byte[] data)
        {
            Byte[] returnVal = new byte[data.Length + 3];

            returnVal[0] = cfgID;
            Byte[] lenBytes = uint16toBytes((UInt16)data.Length);
            returnVal[1] = lenBytes[0];
            returnVal[2] = lenBytes[1];

            for (int i = 0; i < data.Length; i++) returnVal[i + 3] = data[i];
            
            return returnVal;

        }

        Byte[] uint16toBytes(UInt16 x)
        {
            Byte[] returnVal = new byte[2];
            returnVal[0] = (byte)(x & 0xFF);
            returnVal[1] = (byte)(x >> 8);

            return returnVal;
        }

        bool verifyAuthTag(Byte[] msg, Byte[] authTag, Byte[] key, Byte[] nonce)
        {

            Byte[] expectedPoly1305 = genAuthTag(msg, key, nonce);
            return byteArrayEqual(authTag, expectedPoly1305);
        }

        Byte[] genAuthTag(Byte[] msg, Byte[] key, Byte[] nonce)
        {

            
            Byte[] mac = msg;
            if (msg.Length % 16 != 0) mac = msg.Concat(new Byte[16 - (msg.Length % 16)]).ToArray();
            mac = mac.Concat(new Byte[8]).ToArray();
            mac = mac.Concat(fromUInt32ArrayLE(new UInt32[] { (UInt32)msg.Length, 0x0 })).ToArray();

            Byte[] poly1305Key = chacha20Block(key, nonce, 0);
            Byte[] authTag = poly1305(poly1305Key, mac);

            return authTag;

        }
        void sendPairingError(NetworkStream stream, Byte state, Byte errorCode)
        {
            Byte[] TLVResponse = new byte[0];
            appendTLVBytes(ref TLVResponse, 0x06, new byte[] { 0x04 });
            appendTLVBytes(ref TLVResponse, 0x07, new byte[] { 0x02 });
            sendTLVResponse(stream, TLVResponse);
        }
        
        bool byteArrayEqual(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++) if (x[i] != y[i]) return false;

            return true;
        }
        
        void sendTLVResponse(NetworkStream stream, Byte[] responseBytes)
        {
            sendHTTPResponse(stream, "200", "application/pairing+tlv8", responseBytes, responseBytes.Length);
            AddToLogBox("Send Response: " + responseBytes.Length + "\r\n");
        }

        void appendTLVBytes(ref Byte[] bytesIn, Byte TLVkey, Byte[] TLVValue)
        {
            int curPos = 0;
            int n = TLVValue.Length;
            
            do
            {
                bytesIn = bytesIn.Append(TLVkey).ToArray();
                int curLength = n - curPos;
                if (curLength > 255) curLength = 255;
                bytesIn = bytesIn.Append((byte)curLength).ToArray();
                int curEnd = curPos + curLength;
                if (curLength != 0)
                {
                    Byte[] TLVData = new byte[curLength];
                    for (int i = 0; curPos + i < curEnd; i++)
                    {
                        TLVData[i] = TLVValue[curPos + i];
                    }
                    bytesIn = bytesIn.Concat(TLVData).ToArray();
                    curPos = curEnd;
                }

            } while (curPos < n) ;

            return;


        }

        Byte[] findTLVKey(Byte[] TLVIn, Byte TLVKey, int bytesReceived)
        {

            int n = bytesReceived;
            int curPos = 0;
            Byte[] returnVal = new byte[0];
            bool found = false;

            while (curPos < n - 1)
            {
                int curLength = TLVIn[curPos + 1];
                if (TLVIn[curPos] == TLVKey && curLength > 0)
                {
                    found = true;
                    Byte[] TLVData = new byte[curLength];
                    for (int i = curPos + 2; i< curPos+curLength + 2; i++)
                    {
                        TLVData[i - curPos - 2] = TLVIn[i];
                    }
                    returnVal = returnVal.Concat(TLVData).ToArray();
                    
                }
                curPos += curLength + 2;
            }
            if (found)
            {
                return returnVal;
            }
            else return null;



        }
        
        Byte[] byteGenRandom(int length)
        {
            Byte[] x = new byte[length];
            for (int curByte = 0; curByte < length; curByte++) { x[curByte] = (byte)random.Next(); }
            return x;
        }

        private void AddToLogBox(string TextIn)
        {
            label1.BeginInvoke((MethodInvoker)delegate ()
            {
                textBox1.AppendText(TextIn);
            });
        }
        private void updateStatusLbl(string TextIn)
        {
            Status1.BeginInvoke((MethodInvoker)delegate ()
            {
                Status1.Text = TextIn;
            });
        }
        private void sendHTTPResponse(NetworkStream senderStream, string status, string contentType = null, Byte[] data = null, int dataLength = 0)
        {

            String msg = "HTTP/1.1 " + status + " ";
            switch (status)
            {
                case "200":
                    msg += "OK";
                    break;
                case "204":
                    msg += "No Content";
                    break;
            }
            msg += "\r\n";
            if (!(contentType is null))
            {
                msg += "Content-Type: " + contentType + "\r\n";
            }
            if (dataLength != 0)
            {
                msg += "Content-Length: " + dataLength.ToString() + "\r\n";
            }
            msg += "\r\n";
            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);


            if (!(data is null))
            {
                if (dataLength != 0)
                {
                    bytes = bytes.Concat(data.Take(dataLength)).ToArray();
                }
                else
                {
                    bytes = bytes.Concat(data).ToArray();
                }
            }

            senderStream.Write(bytes, 0, bytes.Length);

            return;


        }
        private int findTLVIndex(Byte[] data, int dataLength, Byte typeIn)
        {
            int i = 0;
            while (data[i] != typeIn && i < dataLength)
            {
                i += data[i + 1] + 2;
            }

            return i;

        }
        private UInt64c[] initHexArray(String[] stringsIn)
        {
            UInt64c[] returnArray = new UInt64c[stringsIn.Length];

            int i = 0;
            foreach (string curString in stringsIn)
            {
                returnArray[i++] = UInt64cfromString(curString);
            }
            return returnArray;
        }
        private UInt64c UInt64cfromString(String stringIn)
        {
            UInt64c returnVal;
            returnVal.hi = UInt32.Parse(stringIn.Substring(0, 8), System.Globalization.NumberStyles.HexNumber);
            returnVal.lo = UInt32.Parse(stringIn.Substring(8, 8), System.Globalization.NumberStyles.HexNumber);
            return returnVal;
        }

        Byte[] genSHA512(params Byte[][] list)
        {
            Byte[] toHash = new byte[0];

            foreach(Byte[] i in list)
            {
                toHash = toHash.Concat(i).ToArray();
            }

            return genSHA512(toHash);


        }

        Byte[] genSHA512(UInt32[] msg)
        {
            Byte[] toHash = fromUInt32Array(N);
  
            return genSHA512(toHash); ;

        }

        Byte[] genSHA512(String msg)
        {
            
            return genSHA512(Encoding.UTF8.GetBytes(msg));

        }

        Byte[] genSHA512(Byte[] msg)
        {

            Byte[] returnVal = new Byte[64];
            UInt64c[] H = new UInt64c[HInitial.Length];
            HInitial.CopyTo(H, 0);

            msg = msg.Concat(new Byte[] { 0x80 }).ToArray(); //+= '\u0080'; //append 1 bit, padded with 0s

            int msgLength = (int)((float)msg.Length / 8) + 2;
            int numBlocks = (int)Math.Ceiling(((float)msgLength / 16));
            UInt64c[,] blocks = new UInt64c[numBlocks, 16];

            for (int i = 0; i < numBlocks; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int basePosition = i * 128 + j * 8;
                    for (int k = 0; k < 8; k++)
                    {
                        int curPosition = basePosition + k;
                        if (curPosition >= msg.Length) break;
                        byte curByte = (byte)msg[curPosition];
                        if (k < 4)
                        {
                            blocks[i, j].hi = blocks[i, j].hi | (uint)curByte << ((3 - k) * 8);
                        }
                        else
                        {
                            blocks[i, j].lo = blocks[i, j].lo | (uint)curByte << ((7 - k) * 8);
                        }

                    }
                    //blocks[i, j].lo = (uint)msg[basePosition + 0] << 24 | (uint)msg[basePosition + 1] << 16 | (uint)msg[basePosition + 2] << 8 | (uint)msg[basePosition + 3] << 0;
                    //blocks[i, j].hi = (uint)msg[basePosition + 4] << 24 | (uint)msg[basePosition + 5] << 16 | (uint)msg[basePosition + 6] << 8 | (uint)msg[basePosition + 7] << 0;
                }
            }
            //          blocks[numBlocks - 1, 15].hi = (uint)((msg.Length - 1) * 8) / (uint)Math.Pow(2, 32); //ignore lengths above 2^32 bits, would overflow Arduino
            blocks[numBlocks - 1, 15].lo = (uint)((msg.Length - 1) * 8);

            for (int i = 0; i < numBlocks; i++)
            {
                UInt64c[] W = new UInt64c[80];

                for (int t = 0; t < 16; t++) W[t] = blocks[i, t];
                for (int t = 16; t < 80; t++) W[t] = UInt64cAdd(shaSig1(W[t - 2]), W[t - 7], shaSig0(W[t - 15]), (W[t - 16]));

                UInt64c a = H[0], b = H[1], c = H[2], d = H[3], e = H[4], f = H[5], g = H[6], h = H[7];
                for (int t = 0; t < 80; t++)
                {
                    UInt64c T1 = UInt64cAdd(h, shaSum1(e), shaCH(e, f, g), K[t], W[t]);
                    UInt64c T2 = UInt64cAdd(shaSum0(a), shaMaj(a, b, c));
                    h = g;
                    g = f;
                    f = e;
                    e = UInt64cAdd(d, T1);
                    d = c;
                    c = b;
                    b = a;
                    a = UInt64cAdd(T1, T2);
                }
                H[0] = UInt64cAdd(H[0], a);
                H[1] = UInt64cAdd(H[1], b);
                H[2] = UInt64cAdd(H[2], c);
                H[3] = UInt64cAdd(H[3], d);
                H[4] = UInt64cAdd(H[4], e);
                H[5] = UInt64cAdd(H[5], f);
                H[6] = UInt64cAdd(H[6], g);
                H[7] = UInt64cAdd(H[7], h);
            }

            //foreach (UInt64c i in H)
            //{
            //    AddToLogBox(i.hi.ToString("X8").ToLower());
            //    AddToLogBox(i.lo.ToString("X8").ToLower() + "\r\n");

            //}

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    returnVal[i * 8 + j] = UInt64CGetByte(H[i], j);
                }
            }

            return returnVal;

        }

        private void button1_Click(object sender, EventArgs e)
        {


            CryptoTest.RunWorkerAsync();




        }
        UInt64c ROTR(UInt64c x, int n)
        {
            UInt64c returnVal;

            if (n == 0) return x;
            if (n == 32) return new UInt64c { hi = x.lo, lo = x.hi };

            UInt32 hi = x.hi;
            UInt32 lo = x.lo;

            if (n > 32)
            {
                UInt32 temp = hi;
                hi = lo;
                lo = temp;
                n -= 32;
            }

            returnVal.hi = (hi >> n) | (lo << (32 - n));
            returnVal.lo = (lo >> n) | (hi << (32 - n));

            return returnVal;
        }

        UInt64c UInt64cAdd(UInt64c x, UInt64c y)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo + y.lo;
            returnVal.hi = x.hi + y.hi;
            if (0xffffffff - x.lo < y.lo) //check if lo overflowed
            {
                returnVal.hi += 1;
            }

            return returnVal;
        }

        UInt64c UInt64cAdd(UInt64c x, UInt64c y, UInt64c z)
        {
            return UInt64cAdd(UInt64cAdd(x, y), z);
        }

        UInt64c UInt64cAdd(UInt64c x, UInt64c y, UInt64c z, UInt64c w)
        {
            return UInt64cAdd(UInt64cAdd(x, y, z), w);
        }
        UInt64c UInt64cAdd(UInt64c x, UInt64c y, UInt64c z, UInt64c w, UInt64c v)
        {
            return UInt64cAdd(UInt64cAdd(x, y, z, w), v);
        }
        UInt64c UInt64cAnd(UInt64c x, UInt64c y)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo & y.lo;
            returnVal.hi = x.hi & y.hi;

            return returnVal;
        }
        UInt64c UInt64cXor(UInt64c x, UInt64c y)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo ^ y.lo;
            returnVal.hi = x.hi ^ y.hi;

            return returnVal;
        }

        UInt64c UInt64cXor(UInt64c x, UInt64c y, UInt64c z)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo ^ y.lo ^ z.lo;
            returnVal.hi = x.hi ^ y.hi ^ z.hi;

            return returnVal;
        }

        UInt64c UInt64cNot(UInt64c x)
        {
            UInt64c returnVal;

            returnVal.lo = ~x.lo;
            returnVal.hi = ~x.hi;

            return returnVal;
        }

        UInt64c UInt64cShr(UInt64c x, int n)
        {
            UInt64c returnVal = new UInt64c();

            if (n == 0) returnVal = x;
            else if (n == 32) returnVal.lo = x.hi;
            else if (n > 32) returnVal.lo = x.hi >> (n - 32);
            else
            {
                returnVal.hi = x.hi >> n;
                returnVal.lo = x.lo >> n | x.hi << (32 - n);
            }

            return returnVal;
        }

        UInt64c shaSum0(UInt64c x) { return UInt64cXor(ROTR(x, 28), ROTR(x, 34), ROTR(x, 39)); }
        UInt64c shaSum1(UInt64c x) { return UInt64cXor(ROTR(x, 14), ROTR(x, 18), ROTR(x, 41)); }
        UInt64c shaSig0(UInt64c x) { return UInt64cXor(ROTR(x, 1), ROTR(x, 8), UInt64cShr(x, 7)); }
        UInt64c shaSig1(UInt64c x) { return UInt64cXor(ROTR(x, 19), ROTR(x, 61), UInt64cShr(x, 6)); }
        UInt64c shaCH(UInt64c x, UInt64c y, UInt64c z) { return UInt64cXor(UInt64cAnd(x, y), UInt64cAnd(UInt64cNot(x), z)); }
        UInt64c shaMaj(UInt64c x, UInt64c y, UInt64c z) { return UInt64cXor(UInt64cAnd(x, y), UInt64cAnd(x, z), UInt64cAnd(y, z)); }

        Byte UInt64CGetByte(UInt64c x, int curByte)
        {
            Byte returnVal = 0;

            if (curByte < 4)
            {
                returnVal = (byte)(x.hi >> (3 - curByte) * 8);
            }
            else
            {
                returnVal = (byte)(x.lo >> (7 - curByte) * 8);
            }

            return returnVal;
        }

        Byte[] genVerifier(Byte[] salt, String pass)
        {
            // Byte[] x = genSHA512(Encoding.UTF8.GetString(salt) + pass);

            return null;
        }

        Byte[] modular_pow(uint b, Byte[] e, Byte[] m)
        {
            return null;

        }

        UInt32[] toUInt32Array(Byte[] byteArray)
        {
            int outputSize = (int)Math.Ceiling((decimal)byteArray.Length / 4);
            UInt32[] digits = new uint[outputSize];

            for (int i = 0; i < outputSize; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    digits[i] |= ((UInt32)byteArray[byteArray.Length - (i * 4 + j) - 1]) << (8 * j);

                }
            }

            

            return digits;
        }

        UInt32[] toUInt32ArrayLE(Byte[] byteArray)
        {
            int outputSize = (int)Math.Ceiling((decimal)byteArray.Length / 4);
            UInt32[] digits = new uint[outputSize];

            for (int i = 0; i < outputSize; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int curIndex = i * 4 + j;
                    if (curIndex > byteArray.Length - 1) break;
                    digits[i] |= ((UInt32)byteArray[(i * 4 + j)]) << (8 * j);

                }
            }

            UInt32[] returnVal;
            returnVal = digits;


            return returnVal;
        }

        Byte[] fromUInt32ArrayLE(UInt32[] uint32Array)
        {
            Byte[] returnVal = new byte[uint32Array.Length * 4];

            for (int i = 0; i < uint32Array.Length; i++)
            {
                returnVal[i * 4] = (byte)uint32Array[i];
                returnVal[i * 4 + 1] = (byte)(uint32Array[i] >> 8);
                returnVal[i * 4 + 2] = (byte)(uint32Array[i] >> 16);
                returnVal[i * 4 + 3] = (byte)(uint32Array[i] >> 24);

            }
            return returnVal;
        }

        Byte[] fromUInt32Array(UInt32[] uint32Array)
        {
            return byteArrayReverse(fromUInt32ArrayLE(uint32Array));
        }

        Byte[] fromUInt32ArrayBE(UInt32[] uint32Array)
        {
            Byte[] returnVal = new byte[uint32Array.Length * 4];

            for (int i = 0; i < uint32Array.Length; i++)
            {
                returnVal[i * 4 + 3] = (byte)uint32Array[i];
                returnVal[i * 4 + 2] = (byte)(uint32Array[i] >> 8);
                returnVal[i * 4 + 1] = (byte)(uint32Array[i] >> 16);
                returnVal[i * 4 + 0] = (byte)(uint32Array[i] >> 24);

            }
            return returnVal;
        }
        int32Array int32ArrayAddNoSign(int32Array a, int32Array b, int32Array mod)
        {
            int32Array returnVal;
            returnVal.digits = UInt32AddNoSign(a.digits, b.digits);
            returnVal.negative = false;
            return returnVal;

        }
        UInt32[] UInt32AddNoSign(UInt32[] a, UInt32[] b)
        {
            UInt32[] c;
            byte carry = 0;

            int n = a.Length;
            if (n < b.Length) n = b.Length;
            c = new UInt32[n + 1];

            for (int i = 0; i < n; ++i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                byte newCarry = 0;
                if (i < a.Length) aDigit = a[i];
                if (i < b.Length) bDigit = b[i];
                c[i] = aDigit + bDigit + carry;
                if (0xFFFFFFFF - aDigit < bDigit) newCarry = 1;
                if (carry == 1)
                {
                    if (aDigit + bDigit == 0xFFFFFFFF) newCarry = 1;
                }
                carry = newCarry;


            }
            c[n] = carry;

            return c;


        }

        int int32ArrayCmp(int32Array a, int32Array b)  //return 1 if a>b, -1 if b<a, 0 if a=b
        {

            int signDif = 0;

            if (a.negative & !b.negative) signDif = -1;
            if (!a.negative & b.negative) signDif = 1;

            int n = a.digits.Length;
            if (n < b.digits.Length) n = b.digits.Length;

            for (int i = n - 1; i >= 0; --i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                // byte newCarry = 0;
                if (i < a.digits.Length) aDigit = a.digits[i];
                if (i < b.digits.Length) bDigit = b.digits[i];
                if ((aDigit != 0 | bDigit != 0) & signDif != 0) return signDif;
                if (aDigit > bDigit) return (!a.negative ? 1 : -1);
                if (aDigit < bDigit) return (!a.negative ? -1 : 1);
            }
            return 0;

        }

        int int32ArrayCmpNoSign(int32Array a, int32Array b)  //return 1 if a>b, -1 if b<a, 0 if a=b
        {

            //  int signDif = 0;

            //  if (a.negative & !b.negative) signDif = -1;
            //  if (!a.negative & b.negative) signDif = 1;

            int n = a.digits.Length;
            if (n < b.digits.Length) n = b.digits.Length;

            for (int i = n - 1; i >= 0; --i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                // byte newCarry = 0;
                if (i < a.digits.Length) aDigit = a.digits[i];
                if (i < b.digits.Length) bDigit = b.digits[i];
                //     if ((aDigit != 0 | bDigit != 0) & signDif != 0) return signDif;
                if (aDigit > bDigit) return 1;
                if (aDigit < bDigit) return -1;
            }
            return 0;

        }

        int UInt32ArrayCmpNoSign(UInt32[] a, UInt32[] b)
        {
            int32Array A;
            A.digits = a;
            A.negative = false;

            int32Array B;
            B.digits = b;
            B.negative = false;

            return int32ArrayCmpNoSign(A, B);

        }

        int32Array int32ArrayModSlow(int32Array a, int32Array mod)
        {
            int32Array c;
            c.digits = new uint[mod.digits.Length];
            c.negative = false;

            int cmpResult = int32ArrayCmp(a, mod);
            if (cmpResult == 0) return c;
            if (cmpResult == -1) return a;

            int n = mod.digits.Length;



            int32Array curDigits;
            curDigits.digits = new UInt32[n + 1];
            curDigits.negative = false;
            int32Array curRemainder;
            curRemainder.digits = new UInt32[n + 1];
            curRemainder.negative = false;


            for (int i = a.digits.Length - 1; i >= 0; --i)
            {

                for (int j = n; j > 0; --j)
                {
                    curDigits.digits[j] = curRemainder.digits[j - 1];
                }
                curDigits.digits[0] = a.digits[i];
                while (int32ArrayCmp(curDigits, mod) == 1)
                {
                    curDigits.digits = UInt32ArraySubSimple(curDigits.digits, mod.digits);
                    // updateStatusLbl(curDigits.digits[0].ToString("X8"));
                }
                curRemainder = curDigits;
            }

            c.digits = curRemainder.digits;

            return c;

        }

        UInt32[] UInt32ArrayMod(UInt32[] aIn, UInt32[] modIn)
        {
            int n = aIn.Length;
            int32Array c;
            c.digits = new uint[modIn.Length];
            c.negative = false;

            int32Array a;
            a.digits = aIn;
            a.negative = false;

            int32Array mod;
            mod.digits = modIn;
            mod.negative = false;



            int cmpResult = int32ArrayCmp(a, mod);
            if (cmpResult == 0) return c.digits;
            if (cmpResult == -1)
            {
                for (int i = 0; i < c.digits.Length && i < a.digits.Length; i++)
                {
                    c.digits[i] = a.digits[i];
                }
                return c.digits;
            }

            int32Array curDenominator;
            curDenominator.digits = new UInt32[n * 2];
            curDenominator.negative = false;
            for (int i = 0; i < n; i++)
            {
                if (i < mod.digits.Length)
                {
                    curDenominator.digits[i + n] = mod.digits[i];
                }

            }
            int32Array curRemainder;
            curRemainder.digits = new UInt32[n * 2];
            curRemainder.negative = false;
            for (int i = 0; i < n; i++)
            {
                curRemainder.digits[i] = a.digits[i];
            }

            for (int i = (n * 32) - 1; i >= 0; i--)
            {
                curRemainder = int32ArraySHL(curRemainder);
                if (int32ArrayCmp(curRemainder, curDenominator) >= 0)
                {
                    curRemainder.digits = UInt32ArraySubSimple(curRemainder.digits, curDenominator.digits);
                }
            }

            for (int i = 0; i < mod.digits.Length; i++)
            {
                c.digits[i] = curRemainder.digits[i + n];
            }


            return c.digits;

        }


        void UInt32ArraySHLWords(ref UInt32[] a, int n)
        {
            int len = a.Length;
            if (n > len) n = len;

            for (int i = len - 1; i > (n - 1); --i)
            {
                a[i] = a[i - n];
            }
            for (int i = 0; i < n; i++) a[i] = 0;

            return;
        }

        void UInt32ArraySHRWords(ref UInt32[] a, int n)
        {
            int len = a.Length;
            if (n > len) n = len;

            for (int i = 0; i < (len - n); i++)
            {
                a[i] = a[i + n];
            }
            for (int i = len - n; i < len; i++) a[i] = 0;

            return;
        }
        UInt32[] UInt32ArraySubSimple(UInt32[] a, UInt32[] b) //assumes both positive and a > b
        {
            int n = a.Length;

            UInt32[] c;
            c = new UInt32[n];
            // c.negative = false;

            Byte borrow = 0;

            for (int i = 0; i < n; ++i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                byte newBorrow = 0;
                if (i < a.Length) aDigit = a[i];
                if (i < b.Length) bDigit = b[i];
                c[i] = aDigit - bDigit - borrow;
                if (aDigit < bDigit) newBorrow = 1;
                if (borrow == 1)
                {
                    if (aDigit - bDigit == 0) newBorrow = 1;
                }
                borrow = newBorrow;


            }
            return c;

        }

        int32Array int32ArraySHL(int32Array a)
        {
            int n = a.digits.Length;
            for (int i = n - 1; i > 0; i--)
            {
                a.digits[i] = (a.digits[i] << 1) | (a.digits[i - 1] >> 31);
            }
            a.digits[0] = a.digits[0] << 1;
            return a;

        }

        UInt32[] UInt32ArraySHL(UInt32[] a)
        {
            int32Array A;
            A.digits = a;
            A.negative = false;

            return int32ArraySHL(A).digits;
        }

        int32Array int32ArraySHR(int32Array a)
        {
            //int n = a.digits.Length;
            //for (int i = 0; i < n; i++)
            //{
            //    a.digits[i] = a.digits[i] >> 1;
            //    if (i != n - 1)
            //    {
            //        a.digits[i] |= a.digits[i + 1] << 31;
            //    }
            //}

            a.digits = UInt32ArraySHR(a.digits);

            return a;

        }
        UInt32[] UInt32ArraySHR(UInt32[] a, int n)
        {
                       
            for (int i = 0; i < n; i++)
            {
                a = UInt32ArraySHR(a);
            }
            return a;
        }
        UInt32[] UInt32ArraySHR(UInt32[] a)
        {
            UInt32[] b = new uint[a.Length];
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                b[i] = a[i] >> 1;
                if (i != n - 1)
                {
                    b[i] |= a[i + 1] << 31;
                }
            }

            return b;

        }

        UInt32[] UInt32ArrayMulNoSign(UInt32[] a, UInt32[] b)
        {

            int n = a.Length + b.Length;
            UInt32[] c = new uint[n];
            UInt64c[] p64 = new UInt64c[4];
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    //UInt64c pA64, pB64, pC64, pD64;
                    

                    p64[0].lo = (a[i] & 0xFFFF) * (b[j] & 0xFFFF);
                    p64[0].hi = 0;
                    UInt32 pB = (a[i] & 0xFFFF) * (b[j] >> 16);
                    p64[1].lo = pB << 16;
                    p64[1].hi = pB >> 16;
                    UInt32 pC = (a[i] >> 16) * (b[j] & 0xFFFF);
                    p64[2].lo = pC << 16;
                    p64[2].hi = pC >> 16;
                    p64[3].lo = 0;
                    p64[3].hi = (a[i] >> 16) * (b[j] >> 16);

                    //pA64 = UInt64cAdd(pA64, pB64, pC64, pD64);


                    for (int k = 1; k < 4; k++)
                    {
                        if (0xFFFFFFFF - p64[0].lo < p64[k].lo) p64[0].hi++;
                        p64[0].lo += p64[k].lo;
                        
                    }

                    for (int k = 1; k<4; k++)
                    {
                        p64[0].hi += p64[k].hi;
                    }


                    byte carry = 0;
                    
                    if (0xFFFFFFFF - p64[0].lo < c[i + j]) carry = 1;
                    c[i + j] += p64[0].lo;

                    c[i + j + 1] += carry;
                    if (c[i + j + 1] == 0 && carry == 1)
                    {
                        carry = 1;
                    }
                    else carry = 0;

                    if (0xFFFFFFFF - p64[0].hi < c[i + j + 1]) carry = 1;

                    c[i + j + 1] += p64[0].hi;

                    for (int k = 2; carry == 1; k++)
                    {
                        carry = 0;
                        if (++c[i + j + k] == 0) carry = 1;
                    }

                }
            }

            return c;
        }

        UInt32[] UInt32ArrayMulMod(UInt32[] a, UInt32[] b, UInt32[] mod)
        {
            UInt32[] c = UInt32ArrayMulNoSign(a, b);
            return UInt32ArrayMod(c, mod);
        }

        UInt32[] UInt32PowMod(UInt32[] a, in UInt32[] b, UInt32[] mod)
        {

            int n = b.Length * 32;
            UInt32[] curPower;

            UInt32[] powShift = new UInt32[b.Length];
            b.CopyTo(powShift, 0);

            UInt32[] returnVal = new UInt32[mod.Length];
            returnVal[0] = 1;

            UInt32[][] powers = new UInt32[n][];
            powers[0] = a;



            curPower = a;
            for (int i = 0; i < n; i++)
            {

                if ((powShift[0] & 0x1) == 1)
                {
                    returnVal = UInt32ArrayMulMod(curPower, returnVal, mod);
                }
                curPower = UInt32ArrayMulMod(curPower, curPower, mod);
                //for (int j=curPower.Length -1; j>=0; j--)
                //{
                //    if (curPower[j] > 0)
                //    {
                //        AddToLogBox((j+1).ToString() + "\r\n");
                //        break;
                //    }
                //}
                UInt32ArraySHR(powShift);
                updateStatusLbl(i.ToString());
            }

            return returnVal;


        }

        

        UInt32[] UInt32ArrayReverse(UInt32[] a)
        {
            int n = a.Length;
            UInt32[] c = new UInt32[n];

            for (int i = 0; i < n; i++)
            {
                c[i] = a[n - i - 1];
            }
            return c;
        }

        private void CryptoTest_DoWork(object sender, DoWorkEventArgs e)
        {


            storedx = generatePrivateKey(s, "alice", "password123");
            storedv = generateVerifier(storedx);
            storedk = generateMultiplier();
            storedA = generatePublicA(a);
            storedB = generatePublicB(storedk, storedv, b);
            storedu = generateScrambling(storedA, storedB);
            storedS = generateSessionSecret(storedA, storedv, storedu, b);
            storedK = generateSessionKey(storedS);

            foreach (UInt32 i in storedK)
            {
                AddToLogBox(i.ToString("X8") + "\r\n");
            }



            //Byte[] zeroByte = { 0x0 };


            //BigInteger bigN = new BigInteger(fromUInt32Array(N).Concat(new Byte[] { 0 }).ToArray());
            //BigInteger bigx = new BigInteger(hashBytes.Concat(zeroByte).ToArray());
            //BigInteger bigG = new BigInteger(5);
            //BigInteger bigv = BigInteger.ModPow(bigG, bigx, bigN);

            //AddToLogBox(bigv.ToString("X8") + "\r\n");
        }

        Byte[] byteArrayReverse(Byte[] a)
        {
            Byte[] returnVal = new byte[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                returnVal[i] = a[a.Length - i - 1];
            }
            return returnVal;
        }


        UInt32[] generateVerifier(UInt32[] salt, String authString)
        {
            UInt32[] x = generatePrivateKey(salt, authString);



            return generateVerifier(x);
        }

        UInt32[] generateVerifier(UInt32[] x)
        {
            //UInt32[] g = { generator };
            return UInt32PowModMonty(g, x, N);

        }


        UInt32[] generatePrivateKey(UInt32[] salt, String authString)
        {
            Byte[] saltBytes = fromUInt32Array(salt); 
            Byte[] authBytes = Encoding.ASCII.GetBytes(authString);
            Byte[] pwHash = genSHA512(authBytes);
            Byte[] toHash = saltBytes.Concat(pwHash).ToArray();

            Byte[] hashBytes = genSHA512(toHash);

            return toUInt32Array(hashBytes);
        }

        UInt32[] generatePrivateKey(UInt32[] salt, String userName, String password)
        {
            String authString = userName + ":" + password;
            return generatePrivateKey(salt, authString);
        }

        UInt32[] generateMultiplier()
        {
            UInt32[] gPad = new uint[N.Length];
            gPad[0] = generator;
            Byte[] gBytes = fromUInt32Array(gPad);

            Byte[] NBytes = fromUInt32Array(N);
          
            Byte[] toHash = NBytes.Concat(gBytes).ToArray();
            byte[] hash = genSHA512(toHash);
            UInt32[] returnVal = toUInt32Array(hash);

            return returnVal;
        }

        UInt32[] generatePublicB(UInt32[] k, UInt32[] v, UInt32[] b)
        {

            UInt32[] B = UInt32AddNoSign(UInt32ArrayMulMod(k, v, N), UInt32PowModMonty(g, b, N));
            B = UInt32ArrayMod(B, N);

            return B;
        }

        UInt32[] generateScrambling(UInt32[] A, UInt32[] B)
        {
            Byte[] bytesA = fromUInt32Array(A);
            Byte[] bytesB = fromUInt32Array(B);

            Byte[] toHash = bytesA.Concat(bytesB).ToArray();
            Byte[] hash = genSHA512(toHash);

            UInt32[] u = toUInt32Array(hash);
            return u;
        }

        UInt32[] generatePublicA(UInt32[] a)
        {
            UInt32[] A = UInt32PowModMonty(g, a, N);

            return A;
        }

        UInt32[] generateSessionSecret(UInt32[] A, UInt32[] v, UInt32[] u, UInt32[] b)
        {
            UInt32[] S;

            S = UInt32PowModMonty(v, u, N);
            S = UInt32ArrayMulMod(A, S, N);

            S = UInt32PowModMonty(S, b, N);

            return S;

        }

        UInt32[] generateSessionKey(UInt32[] S)
        {
            Byte[] toHash = fromUInt32Array(storedS);

            Byte[] hash = genSHA512(toHash);

            return toUInt32Array(hash);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Byte[] toHash = Encoding.UTF8.GetBytes(textBox2.Text);
            Byte[] hash = genSHA512(toHash);

            foreach (Byte i in hash)
            {
                AddToLogBox(i.ToString("X2") + " ");
            }
            AddToLogBox("\r\n");

        }

        void UInt32ArrayDiv(UInt32[] A, UInt32[] B, out UInt32[] q, out UInt32[] r)
        {
            int n = A.Length;
            UInt32[] c;
            c = new uint[B.Length];


            UInt32[] a = new UInt32[A.Length];
            A.CopyTo(a, 0);


            int cmpResult = UInt32ArrayCmpNoSign(a, B);
            if (cmpResult == 0)
            {
                q = new UInt32[] { 1 };
                r = new uint[] { 0 };
            }
            if (cmpResult == -1)
            {
                q = new UInt32[] { 0 };
                r = new UInt32[B.Length];

                for (int i = 0; i < r.Length && i < a.Length; i++)
                {
                    r[i] = a[i];
                }
                return;
            }

            UInt32[] curDenominator = new UInt32[n * 2];

            for (int i = 0; i < n; i++)
            {
                if (i < B.Length)
                {
                    curDenominator[i + n] = B[i];
                }

            }
            UInt32[] curRemainder = new UInt32[n * 2];
            for (int i = 0; i < n; i++)
            {
                curRemainder[i] = a[i];
            }
            q = new UInt32[A.Length + B.Length];
            for (int i = (n * 32) - 1; i >= 0; i--)
            {
                curRemainder = UInt32ArraySHL(curRemainder);
                if (UInt32ArrayCmpNoSign(curRemainder, curDenominator) >= 0)
                {
                    curRemainder = UInt32ArraySubSimple(curRemainder, curDenominator);
                    q[i / 32] |= (UInt32)0x1 << (i - i / 32); 
                }
            }

            
            r = new UInt32[B.Length];

            for (int i = 0; i < B.Length; i++)
            {
                r[i] = curRemainder[i + n];
            }

            return;
        }

        UInt32[] UInt32ArrayGetInverse(UInt32[] u, UInt32[] v)
        {
            UInt32[] u1, v1, t1, u3, v3, q, r, inv;

            u3 = new uint[u.Length];
            u.CopyTo(u3, 0);

            v3 = new uint[v.Length];
            v.CopyTo(v3, 0);

            u1 = new uint[] { 1 };
            v1 = new uint[] { 0 };

            bool iter = true;

            while (!UInt32ArrayIsZero(v3))
            {
                UInt32ArrayDiv(u3, v3, out q, out r);
                t1 = UInt32AddNoSign(u1, UInt32ArrayMulNoSign(q, v1));
                UInt32ArrayShrink(ref t1);
                u1 = v1;
                v1 = t1;
                u3 = v3;
                v3 = r;

                iter = !iter;
            }

            if (!iter) inv = UInt32ArraySubSimple(v1, u1);
            else inv = u1;

            return inv;


        }

        bool UInt32ArrayIsZero (UInt32[] x) 
        {
            foreach (UInt32 i in x) if (i != 0) return false;
            return true;
        }
        
        void UInt32ArrayShrink (ref UInt32[] x)
        {
            int msw;
            UInt32[] y;

            for (msw = x.Length - 1; msw > 0; msw--)
            {
                if (x[msw] != 0) break;
            }
            if (msw < x.Length -1)
            {
                y = new UInt32[msw + 1];
                for (int i = 0; i < y.Length; i++)
                {
                    y[i] = x[i];
                }
                x = y;
            }
            return;
        }
        UInt32[] UInt32ArrayMulMonty(UInt32[] x, UInt32[] y, UInt32[] m, UInt32[] m_prime)
        {

            int n = m.Length;
            UInt32[] A = new UInt32[m.Length];

            for (int i = 0; i < n; i++)
            {
                UInt32 xi = (i < x.Length) ? x[i] : 0;
                UInt32[] u = new uint[] { (A[0] + xi * y[0]) * m_prime[0] };
                UInt32[] xiy = UInt32ArrayMulNoSign(new uint[] { xi }, y);
                UInt32[] uim = UInt32ArrayMulNoSign(u, m);
                A = UInt32AddNoSign(A, UInt32AddNoSign(xiy, uim));
                UInt32ArraySHRWords(ref A, 1);
                UInt32ArrayShrink(ref A);

            }
            if (UInt32ArrayCmpNoSign(A, m) >= 0) A = UInt32ArraySubSimple(A, m);
            UInt32ArrayShrink(ref A);
            return A;
        }
        UInt32[] UInt32PowModMonty(UInt32[] x, UInt32[] e, UInt32[] m)
        {

            if (UInt32ArrayCmpNoSign(x, m) != -1) x = UInt32ArrayMod(x, m);

            updateStatusLbl("Started");
            UInt32[] R = new UInt32[m.Length + 1];
            R[m.Length] = 0x1;
            //UInt32[] R_inv = UInt32ArrayGetInverse(R, m);
            UInt32[] ba = new UInt32[] { 0x0, 0x1 };
            UInt32[] m_inv = UInt32ArrayGetInverse(m, ba);
            UInt32[] m_prime = UInt32ArraySubSimple(R, m_inv);
            UInt32ArrayShrink(ref m_prime);
            
            UInt32[] A = UInt32ArrayMod(R, m);
            UInt32[] R2 = UInt32ArrayMulMod(A, A, m);
            UInt32[] x_bar = UInt32ArrayMulMonty(x, R2, m, m_prime);
            int loopCount = 0;


            for (int i = e.Length - 1; i >= 0; i--)
            {
                UInt32 eWord = e[i];
                for (int j = 31; j >= 0; j--)
                {
                    A = UInt32ArrayMulMonty(A, A, m, m_prime);
                    if ((eWord & 0x80000000) == 0x80000000)
                    {
                        A = UInt32ArrayMulMonty(A, x_bar, m, m_prime);
                    }
                    eWord <<= 1;
                    updateStatusLbl((++loopCount).ToString());

                }
            }
            A = UInt32ArrayMulMonty(A, new uint[] { 0x1 }, m, m_prime);

            return A;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MontyTest.RunWorkerAsync();
        }

        private void MontyTest_DoWork(object sender, DoWorkEventArgs e)
        {
            UInt32[] testResult = UInt32PowModMonty(g, a, N);

            foreach (UInt32 i in testResult)
            {
                //AddToLogBox(i.ToString() + " " + i.ToString("X8") + "\r\n");
                AddToLogBox(i.ToString("X8") + "\r\n");
            }
        }

        Byte[] genHKDFSHA512(Byte[] IKM, Byte[] salt, Byte[] info, int L)
        {
            

            Byte[] PRK = genHMACSHA512(salt, IKM); //extract

            //AddToLogBox("PRK:\r\n");
            //foreach (Byte i in PRK)
            //{
            //    AddToLogBox(i.ToString("X2") + "\r\n");

            //}

            int n = (int)Math.Ceiling((decimal)L / PRK.Length);

            Byte[] oldT = new byte[0];
            Byte[] T = new byte[0];

            for (int i = 1; i < n + 1; i++)
            {
                T = genHMACSHA512(PRK, oldT, info, new byte[] { (byte)i });
                oldT = T;
            }

            Byte[] returnVal = new byte[L];
            for (int i = 0; i < L; i++) returnVal[i] = T[i];

            return returnVal;
        }

        Byte[] genHKDFSHA512(Byte[] IKM, String salt, String info, int L)
        {
            Byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            Byte[] infoBytes = Encoding.UTF8.GetBytes(info);

            return genHKDFSHA512(IKM, saltBytes, infoBytes, L);

        }

            private void button4_Click(object sender, EventArgs e)
        {
            //Byte[] IKM = new byte[] { 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b };
            //Byte[] salt = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c };
            //Byte[] info = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9 };


            //Byte[] testResult = genHKDFSHA512(IKM, salt, info, 42);


            //UInt32[] testVector = new UInt32[] { 0x11111111, 0x01020304, 0x9b8d6f43, 0x01234567 };

            //chacha20Quarter(ref testVector[0], ref testVector[1], ref testVector[2], ref testVector[3]);

            
            byte[] testKey = new byte[] {   0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f};
            byte[] testNonce = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0x00 };
            testKey = new byte[] {          0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
                                            0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f};
            testNonce = new byte[] {        0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

            //  testKey = new byte[] {          0x85, 0xd6, 0xbe, 0x78, 0x57, 0x55, 0x6d, 0x33, 0x7f, 0x44, 0x52, 0xfe, 0x42, 0xd5, 0x06, 0xa8,
            //                            0x01, 0x03, 0x80, 0x8a, 0xfb, 0x0d, 0xb2, 0xfd, 0x4a, 0xbf, 0xf6, 0xaf, 0x41, 0x49, 0xf5, 0x1b};

            testKey = new byte[] {  0x1c, 0x92, 0x40, 0xa5, 0xeb, 0x55, 0xd3, 0x8a, 0xf3, 0x33, 0x88, 0x86, 0x04, 0xf6, 0xb5, 0xf0,
                                    0x47, 0x39, 0x17, 0xc1, 0x40, 0x2b, 0x80, 0x09, 0x9d, 0xca, 0x5c, 0xbc, 0x20, 0x70, 0x75, 0xc0};

            UInt32[] prevState = null;

            Byte[] testBytes = Encoding.UTF8.GetBytes("Ladies and Gentlemen of the class of '99: If I could offer you only one tip for the future, sunscreen would be it.");
            testBytes = Encoding.UTF8.GetBytes("Cryptographic Forum Research Group");

            //Byte[] testResult = chacha20(testKey, testNonce, testBytes, 1);
            //Byte[] testResult = poly1305(testKey, testBytes);
            Byte[] testResult = chacha20Block(testKey, testNonce, 0, ref prevState);
            //AddToLogBox("OKM:\r\n");
            foreach (UInt32 i in testResult)
            {
                AddToLogBox(i.ToString("X8") + "\r\n");

            }
            //AddToLogBox(Encoding.UTF8.GetString(testResult) + "\r\n");

            //testResult = chacha20(testKey, testNonce, testResult, 1);

            //AddToLogBox(Encoding.UTF8.GetString(testResult) + "\r\n");

        }

        Byte[] genHMACSHA512(Byte[] K, params Byte[][] list)
        {

            const Byte B = 128;
            const Byte L = 64;

            if (K.Length > B) K = genSHA512(K);

            {
                Byte[] KPad = new byte[B];
                for (int i = 0; i < K.Length; i++) KPad[i] = K[i];
                K = KPad;
            }


            Byte[] toHash = new byte[0];

            foreach (Byte[] i in list)
            {
                toHash = toHash.Concat(i).ToArray();
            }

            Byte[] K_ipad = new byte[B], K_opad = new byte[B];
            for (int i = 0; i < B; i++) K_ipad[i] = (byte)(K[i] ^ 0x36);
            for (int i = 0; i < B; i++) K_opad[i] = (byte)(K[i] ^ 0x5c);

            return genSHA512(K_opad, genSHA512(K_ipad, toHash));


        }

        Byte[] chacha20Block(Byte[] key, Byte[] nonce_in, UInt32 count)
        {
            UInt32[] prevState = null;
            return chacha20Block(key, nonce_in, count, ref prevState);
        }

        Byte[] chacha20Block(Byte[] key, Byte[] nonce_in, UInt32 count, ref UInt32[] state_in)
        {

            UInt32[] state = new uint[16];
            if (state_in == null)
            {
                state_in = new uint[16];
                Byte[] nonce = new Byte[12];
                for (int i = 0; i < nonce_in.Length; i++)
                {
                    nonce[nonce.Length - i - 1] = nonce_in[nonce_in.Length - i - 1];
                }

                state_in[0] = 0x61707865;
                state_in[1] = 0x3320646e;
                state_in[2] = 0x79622d32;
                state_in[3] = 0x6b206574;

                UInt32[] keyUInt32 = toUInt32ArrayLE(key);
                for (int i = 0; i < keyUInt32.Length; i++) state_in[i + 4] = keyUInt32[i];



                UInt32[] nonceUInt32 = toUInt32ArrayLE(nonce);
                for (int i = 0; i < nonceUInt32.Length; i++) state_in[i + 13] = nonceUInt32[i];
            }
            
            state_in[12] = count;
            for (int i = 0; i < state.Length; i++) state[i] = state_in[i];
           

            for (int i = 0; i < 10; i++)
            {
                chacha20Quarter(ref state[0], ref state[4], ref state[8], ref state[12]);
                chacha20Quarter(ref state[1], ref state[5], ref state[9], ref state[13]);
                chacha20Quarter(ref state[2], ref state[6], ref state[10], ref state[14]);
                chacha20Quarter(ref state[3], ref state[7], ref state[11], ref state[15]);
                chacha20Quarter(ref state[0], ref state[5], ref state[10], ref state[15]);
                chacha20Quarter(ref state[1], ref state[6], ref state[11], ref state[12]);
                chacha20Quarter(ref state[2], ref state[7], ref state[8], ref state[13]);
                chacha20Quarter(ref state[3], ref state[4], ref state[9], ref state[14]);
            }

            for (int i = 0; i < state.Length; i++) state[i] += state_in[i];

            return fromUInt32ArrayLE(state);

        }

        void rol(ref UInt32 x, int n)
        {
            x = (x << n | x >> (32 - n));
        }

        void chacha20Quarter (ref UInt32 a, ref UInt32 b, ref UInt32 c, ref UInt32 d)
        {
            a += b; d ^= a; rol(ref d, 16);
            c += d; b ^= c; rol(ref b, 12);
            a += b; d ^= a; rol(ref d, 8);
            c += d; b ^= c; rol(ref b, 7); 
        }

        Byte[] chacha20 (Byte[] key, Byte[] nonce, Byte[] plainText, UInt32 initialCounter)
        {
            Byte[] returnVal = new byte[plainText.Length];
            UInt32 nBlocks = (UInt32)Math.Ceiling((decimal)plainText.Length / 64);
            UInt32[] prevState = null;
            for (UInt32 i = 0; i<nBlocks; i++)
            {
                Byte[] curBlock = chacha20Block(key, nonce, i + initialCounter, ref prevState);
                UInt32 curStart = i * 64;
                Byte blockLen = (i != nBlocks - 1) ? (byte)64 : (byte)(plainText.Length - curStart);
                for (UInt32 j = 0; j<blockLen;j++)
                {
                    returnVal[curStart + j] = (byte)(plainText[curStart + j] ^ curBlock[j]);
                }
            }

            return returnVal;


        }

       

        Byte[] poly1305 (Byte[] key, Byte[] msg)
        {

            Byte[] rBytes = new byte[16];
            Byte[] sBytes = new byte[16];

            for (int i = 0; i < rBytes.Length; i++) rBytes[i] = key[i];
            for (int i = 0; i < sBytes.Length; i++) sBytes[i] = key[i+16];

            UInt32[] p = new UInt32[] { 0x3, 0xffffffff, 0xffffffff, 0xffffffff, 0xfffffffb };
            p = UInt32ArrayReverse(p);

            UInt32[] A = new uint[] { 0 };

            rBytes[3] &= 15;
            rBytes[7] &= 15;
            rBytes[11] &= 15;
            rBytes[15] &= 15;
            rBytes[4] &= 252;
            rBytes[8] &= 252;
            rBytes[12] &= 252;
            
            UInt32[] r = toUInt32ArrayLE(rBytes);
            UInt32[] s = toUInt32ArrayLE(sBytes);

            UInt32 nBlocks = (UInt32)Math.Ceiling((decimal)msg.Length / 16);

            for (UInt32 i = 0; i< nBlocks; i++)
            {
                
                UInt32 curStart = i * 16;
                Byte blockLen = (i != nBlocks - 1) ? (byte)16 : (byte)(msg.Length - curStart);
                Byte[] curBlock = new byte[blockLen + 1];
                
                for (int j = 0; j < blockLen; j++) curBlock[j] = msg[curStart + j]; //separate out current block
                curBlock[blockLen] = 0x01;
                UInt32[] curBlockUInt32 = toUInt32ArrayLE(curBlock);

                A = UInt32AddNoSign(A, curBlockUInt32);
                A = UInt32ArrayMulMod(A, r, p);
            }

            A = UInt32AddNoSign(A, s);
            Byte[] A_Bytes = fromUInt32ArrayLE(A);

            Byte[] returnVal = new byte[16];
            for (int i = 0; i < 16; i++) returnVal[i] = A_Bytes[i];

            return returnVal;


        }

        const int ed25519BitLength = 256;

        UInt32[] ed25519Bx = new UInt32[] { 0x216936d3, 0xcd6e53fe, 0xc0a4e231, 0xfdd6dc5c, 0x692cc760, 0x9525a7b2, 0xc9562d60, 0x8f25d51a };
        UInt32[] ed25519By = new UInt32[] { 0x66666666, 0x66666666, 0x66666666, 0x66666666, 0x66666666, 0x66666666, 0x66666666, 0x66666658 };
        UInt32[] ed25519d = new UInt32[] {  0x52036cee, 0x2b6ffe73, 0x8cc74079, 0x7779e898, 0x00700a4d, 0x4141d8ab, 0x75eb4dca, 0x135978a3 };
        UInt32[] ed25519i = new UInt32[] {  0x2b832480, 0x4fc1df0b, 0x2b4d0099, 0x3dfbd7a7, 0x2f431806, 0xad2fe478, 0xc4ee1b27, 0x4a0ea0b0 };
        UInt32[] ed25519L = new uint[] {    0x10000000, 0x00000000, 0x00000000, 0x00000000, 0x14def9de, 0xa2f79cd6, 0x5812631a, 0x5cf5d3ed };
        pointUInt32 ed25519B;
        pointExtUInt32 ed25519G;
        UInt32[] ed25519p;

        void init_ed25519()
        {
            ed25519p = new UInt32[] { 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x80000000 };
            ed25519p = UInt32ArraySubSimple(ed25519p, new UInt32[] { 19 });

            ed25519B.x = UInt32ArrayReverse(ed25519Bx);
            ed25519B.y = UInt32ArrayReverse(ed25519By);
            ed25519d = UInt32ArrayReverse(ed25519d);
            ed25519i = UInt32ArrayReverse(ed25519i);
            ed25519L = UInt32ArrayReverse(ed25519L);

            ed25519G.X.negative = false; ed25519G.Y.negative = false; ed25519G.Z.negative = false; ed25519G.T.negative = false; 

            ed25519G.Y.digits = UInt32ArrayMulMod(new UInt32[] { 0x4 }, ed25519modInv(new UInt32[] { 0x5 }), ed25519p);
            ed25519G.X.digits = ed25519recover_x(ed25519G.Y.digits, 0);
            ed25519G.Z.digits = new uint[] { 0x1 };
            ed25519G.T = int32ArrayMulMod(ed25519G.X, ed25519G.Y, ed25519p);

            //addBigIntToLogBox(ed25519G.X);



            
        }

        Byte[] ed25519sign(Byte[] secret, Byte[] msg)
        {
            Byte[] prefix;
            UInt32[] a;
            Byte[] A = ed25519PublicKey(secret, out prefix, out a);
            UInt32[] r = ed25519sha512modL(prefix, msg);
            pointExtUInt32 R = ed25519ScalarMul(r, ed25519G);
            Byte[] Rs = ed25519compress(R);
            UInt32[] h = ed25519sha512modL(Rs, A, msg);
            UInt32[] s = UInt32ArrayMod(UInt32AddNoSign(r, UInt32ArrayMulNoSign(h, a)), ed25519L);

            Byte[] sBytes = fromUInt32ArrayLE(s);
            Byte[] signature = new Byte[64];

            for (int i = 0; i < 32; i++) signature[i] = Rs[i];
            for (int i = 0; i < 32; i++) signature[i + 32] = sBytes[i];

            return signature;

        }

        bool ed25519verify(Byte[] publicKey, Byte[] msg, Byte[] signature)
        {
            pointExtUInt32 A = ed25519decompress(publicKey);
            Byte[] Rs = new byte[32];
            for (int i = 0; i < 32; i++) Rs[i] = signature[i];
            pointExtUInt32 R = ed25519decompress(Rs);

            Byte[] sBytes = new byte[32];
            for (int i = 0; i < 32; i++) sBytes[i] = signature[i + 32];
            UInt32[] s = toUInt32ArrayLE(sBytes);
            
            UInt32[] h = ed25519sha512modL(Rs, publicKey, msg);

            pointExtUInt32 sB = ed25519ScalarMul(s, ed25519G);
            pointExtUInt32 hA = ed25519ScalarMul(h, A);

            return ed25519pointEqual(sB, ed25519pointAdd(R, hA));


        }

        bool ed25519pointEqual(pointExtUInt32 P, pointExtUInt32 Q)
        {
            int32Array result;
            
            result = int32ArraySub(int32ArrayMul(P.X, Q.Z), int32ArrayMul(Q.X, P.Z));
            result = int32ArrayMod(result, ed25519p);
            if (!UInt32ArrayIsZero(result.digits)) return false;

            result = int32ArraySub(int32ArrayMul(P.Y, Q.Z), int32ArrayMul(Q.Y, P.Z));
            result = int32ArrayMod(result, ed25519p);
            if (!UInt32ArrayIsZero(result.digits)) return false;

            return true;
        }

        UInt32[] ed25519sha512modL(params Byte[][] list)
        {
            Byte[] hashBytes = genSHA512(list);
            UInt32[] hash = toUInt32ArrayLE(hashBytes);
            UInt32[] hashMod = UInt32ArrayMod(hash, ed25519L);

            return hashMod;
            

        }


        UInt32[] ed25519recover_x(UInt32[] y, int sign)
        {
            UInt32[] y2 = UInt32ArrayMulNoSign(y, y);

            UInt32[] dy2p1 = UInt32AddNoSign(UInt32ArrayMulNoSign(ed25519d, y2), new uint[] { 0x1 });
            UInt32[] invy2 = ed25519modInv(dy2p1);

            UInt32[] y2m1 = UInt32ArraySubSimple(y2, new uint[] { 0x01 });
            UInt32[] x2 = UInt32ArrayMulNoSign(y2m1, invy2);
            if (UInt32ArrayIsZero(x2)) return new uint[] { 0x0 };

            UInt32[] pPlus3 = UInt32AddNoSign(ed25519p, new UInt32[] { 0x03 });
            UInt32[] x = UInt32PowModMonty(x2, UInt32ArraySHR(pPlus3, 3), ed25519p);

            UInt32[] xTest = UInt32ArraySubMod(UInt32ArrayMulNoSign(x, x), x2, ed25519p);
            if (!UInt32ArrayIsZero(xTest)) x = UInt32ArrayMulMod(x, ed25519i, ed25519p);

            if ((x[0] & 1) != sign) x = UInt32ArraySubSimple(ed25519p, x);

            return x;

        }


        UInt32[] ed25519modInv(UInt32[] x)
        {
            //if (UInt32ArrayCmpNoSign(x, ed25519p) != -1) x = UInt32ArrayMod(x, ed25519p);
            return UInt32PowModMonty(x, UInt32ArraySubSimple(ed25519p, new UInt32[] { 0x2 }), ed25519p);
        }

        struct pointUInt32
        {
            public UInt32[] x;
            public UInt32[] y;
        }

        struct pointExtUInt32
        {
            public int32Array X;
            public int32Array Y;
            public int32Array Z;
            public int32Array T;
        }

        Byte[] ed25519PublicKey(byte[] signingKey)
        {
            Byte[] temp;
            UInt32[] temp2;
            return ed25519PublicKey(signingKey, out temp, out temp2);
        }


        Byte[] ed25519PublicKey(byte[] signingKey, out Byte[] prefix, out UInt32[] a)
        {
            Byte[] h = genSHA512(signingKey);
            Byte[] aBytes = new Byte[ed25519BitLength / 8];
            for (int i = 0; i < aBytes.Length; i++) aBytes[i] = h[i];
            aBytes[0] &= 0xF8;
            aBytes[31] &= 0x7F;
            aBytes[31] |= 0x40;

            a = toUInt32ArrayLE(aBytes);

            pointExtUInt32 bigA = ed25519ScalarMul(a, ed25519G);

            prefix = new byte[32];
            for (int i = 0; i < prefix.Length; i++) prefix[i] = h[i + 32];

            return ed25519compress(bigA);

        }

        Byte[] ed25519compress(pointExtUInt32 P)
        {

            int32Array zinv;
            zinv.digits = ed25519modInv(int32ArrayMod(P.Z, ed25519p).digits);
            zinv.negative = false;

            UInt32[] x = int32ArrayMulMod(P.X, zinv, ed25519p).digits;
            UInt32[] y = int32ArrayMulMod(P.Y, zinv, ed25519p).digits;

            y[7] |= x[0] << 31;

            UInt32[] short_y = new UInt32[8];
            for (int i = 0; i < 8; i++) short_y[i] = y[i];

            return fromUInt32ArrayLE(short_y);

        }

        pointExtUInt32 ed25519decompress(Byte[] s)
        {
            UInt32[] y = toUInt32ArrayLE(s);
            int sign = (int)(y[7] >> 31);
            y[7] &= 0x7fffffff;
            UInt32[] x = ed25519recover_x(y, sign);

            pointExtUInt32 returnVal;
            returnVal.X.negative = false; returnVal.Y.negative = false; returnVal.Z.negative = false; returnVal.T.negative = false;

            returnVal.X.digits = x;
            returnVal.Y.digits = y;
            returnVal.Z.digits = new uint[] { 0x1 };
            returnVal.T.digits = UInt32ArrayMulMod(x, y, ed25519p);

            return returnVal;

        }

        pointExtUInt32 ed25519ScalarMul(UInt32[] s_in, pointExtUInt32 P)
        {

            UInt32[] s = new UInt32[s_in.Length];
            for (int i = 0; i < s.Length; i++) s[i] = s_in[i];

            pointExtUInt32 Q;
            Q.X.digits = new uint[] { 0x0 }; Q.Y.digits = new uint[] { 0x1 }; Q.Z.digits = new uint[] { 0x1 }; Q.T.digits = new uint[] { 0x0 };
            Q.X.negative = false; Q.Y.negative = false; Q.Z.negative = false; Q.T.negative = false;

            while (!UInt32ArrayIsZero(s))
            {
                
                if ((s[0] & 0x1) == 1) Q = ed25519pointAdd(Q, P);
                P = ed25519pointAdd(P, P);
                s = UInt32ArraySHR(s);
            }

            return Q;

        }

        pointExtUInt32 ed25519pointAdd(pointExtUInt32 P, pointExtUInt32 Q)
        {
            int32Array d;
            d.digits = ed25519d;
            d.negative = false;
            
            int32Array A = int32ArrayMulMod(int32ArraySub(P.Y, P.X), int32ArraySub(Q.Y, Q.X), ed25519p);
            int32Array B = int32ArrayMulMod(int32ArrayAdd(P.Y, P.X), int32ArrayAdd(Q.Y, Q.X), ed25519p);
            
            int32Array C = int32ArrayMul(int32ArrayMul(P.T, Q.T), d);
            C.digits = UInt32ArraySHL(C.digits);
            C = int32ArrayMod(C, ed25519p);
            
            int32Array D = int32ArrayMul(P.Z, Q.Z);
            D.digits = UInt32ArraySHL(D.digits);
            D = int32ArrayMod(D, ed25519p);

            int32Array E = int32ArraySub(B, A);
            int32Array F = int32ArraySub(D, C);
            int32Array G = int32ArrayAdd(D, C);
            int32Array H = int32ArrayAdd(B, A);

            

            pointExtUInt32 returnVal;
            returnVal.X = int32ArrayMul(E, F);
            returnVal.Y = int32ArrayMul(G, H);
            returnVal.Z = int32ArrayMul(F, G);
            returnVal.T = int32ArrayMul(E, H);

            return returnVal;
        }

        UInt32[] UInt32ArraySubMod(UInt32[] a, UInt32[] b, UInt32 [] p)
        {
            switch (UInt32ArrayCmpNoSign(a, b))
            {
                case 0: return new uint[] { 0x0 };
                case 1:
                    {
                        return UInt32ArrayMod(UInt32ArraySubSimple(a, b), p);
                    }
                case -1:
                    {
                        return UInt32ArrayMod(UInt32ArraySubSimple(p, UInt32ArrayMod(UInt32ArraySubSimple(b, a), p)),p);
                    }

            }
            return null;
        }

        int32Array int32ArraySub(UInt32[] a, UInt32[] b)
        {
            int32Array x;
            x.digits = a;
            x.negative = false;

            int32Array y;
            y.digits = b;
            y.negative = false;

            return int32ArraySub(x, y);

        }
        int32Array int32ArrayAdd(UInt32[] a, UInt32[] b)
        {
            int32Array x;
            x.digits = a;
            x.negative = false;

            int32Array y;
            y.digits = b;
            y.negative = false;

            return int32ArrayAdd(x, y);

        }

        int32Array int32ArraySub(int32Array a, int32Array b)
        {
            int32Array returnVal;

            b.negative = !b.negative;
            returnVal = int32ArrayAdd(a, b);
            b.negative = !b.negative;

            return returnVal;
        }

        int32Array int32ArrayAdd(int32Array a, int32Array b)
        {
            int32Array returnVal;
            returnVal.digits = new UInt32[] { 0x0 };
            returnVal.negative = false;

            
            if (a.negative)
            {
                if (b.negative)
                {
                    returnVal.negative = true;
                    returnVal.digits = UInt32AddNoSign(a.digits, b.digits);
                }
                else
                {
                    switch (UInt32ArrayCmpNoSign(a.digits, b.digits))
                    {
                        case -1:
                            {
                                returnVal.negative = false;
                                returnVal.digits = UInt32ArraySubSimple(b.digits, a.digits);
                                break;
                            }
                        case 1:
                            {
                                returnVal.negative = true;
                                returnVal.digits = UInt32ArraySubSimple(a.digits, b.digits);
                                break;
                            }
                    }
                }
            }
            else
            {
                if (b.negative)
                {
                    switch (UInt32ArrayCmpNoSign(a.digits, b.digits))
                    {
                        case -1:
                            {
                                returnVal.negative = true;
                                returnVal.digits = UInt32ArraySubSimple(b.digits, a.digits);
                                break;
                            }
                        case 1:
                            {
                                returnVal.negative = false;
                                returnVal.digits = UInt32ArraySubSimple(a.digits, b.digits);
                                break;
                            }
                    }
                }
                
                else
                {
                    returnVal.negative = false;
                    returnVal.digits = UInt32AddNoSign(a.digits, b.digits);
                }
            }
            return returnVal;
        }

        int32Array int32ArrayMulMod(int32Array a, int32Array b, UInt32[] mod)
        {
            int32Array returnVal;
            returnVal.digits = UInt32ArrayMulMod(a.digits, b.digits, mod);
            if (a.negative == b.negative) returnVal.negative = false;
            else returnVal.negative = true;

            if (UInt32ArrayIsZero(returnVal.digits))
            {
                returnVal.negative = false;
            }
            else
            {
                if (returnVal.negative == true)
                {
                    returnVal.digits = UInt32ArraySubSimple(mod, returnVal.digits);
                    returnVal.negative = false;
                }

            }
            return returnVal;
        }

        int32Array int32ArrayMul(int32Array a, int32Array b)
        {
            int32Array returnVal;
            returnVal.digits = UInt32ArrayMulNoSign(a.digits, b.digits);
            
            if ((a.negative == b.negative) || (UInt32ArrayIsZero(returnVal.digits))) returnVal.negative = false;
            else returnVal.negative = true;

            return returnVal;
        }

        int32Array int32ArrayMod(int32Array a, UInt32[] mod)
        {
            int32Array returnVal;
            returnVal.digits = UInt32ArrayMod(a.digits, mod);
            returnVal.negative = a.negative;
            if (UInt32ArrayIsZero(returnVal.digits))
            {
                returnVal.negative = false;
            }
            if (returnVal.negative == true)
            {
                returnVal.digits = UInt32ArraySubSimple(mod, returnVal.digits);
                returnVal.negative = false;
            }
            

            return returnVal;

        }

        void addBigIntToLogBox(int32Array x)
        {
            Byte[] xBytes = fromUInt32ArrayLE(x.digits);
            xBytes = xBytes.Append((byte)0x00).ToArray();

            BigInteger xBigInt = new BigInteger(xBytes);

            if (x.negative) xBigInt = xBigInt * -1;

            AddToLogBox(xBigInt.ToString() + "\r\n");

       

        }

        void addBigIntToLogBox(UInt32[] x_in)
        {
            int32Array x;
            x.digits = x_in;
            x.negative = false;
            addBigIntToLogBox(x);

        }
    }
}

