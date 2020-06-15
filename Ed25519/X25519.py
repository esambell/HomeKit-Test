import array as arr
import math
p = 2**255 - 19

def decodeLittleEndian(b, bits):
    return sum([b[i] << 8*i for i in range(((bits+7)//8))])

def decodeUCoordinate(u, bits):
    #u_list = [ord(b) for b in u]
    u_list = u.tolist()
    # Ignore any unused bits.
    if bits % 8:
        u_list[-1] &= (1<<(bits%8))-1
    return decodeLittleEndian(u_list, bits)

def encodeUCoordinate(u, bits):
    u = u % p
    return ''.join([chr((u >> 8*i) & 0xff)
                    for i in range(((bits+7)//8))])

def decodeScalar25519(k):
    #k_list = [ord(b) for b in k]
    k_list = k.tolist()
    k_list[0] &= 248
    k_list[31] &= 127
    k_list[31] |= 64
    return decodeLittleEndian(k_list, 255)

def decodeScalar448(k):
    k_list = [ord(b) for b in k]
    k_list[0] &= 252
    k_list[55] |= 128
    return decodeLittleEndian(k_list, 448)

def cswap(swap, x_2, x_3):

    mask = 0 - swap

    dummy = mask & (x_2 ^ x_3)
    x_2 = x_2 ^ dummy
    x_3 = x_3 ^ dummy
    return (x_2, x_3)


def X25519(k, u):
    x_1 = u % p
    x_2 = 1
    z_2 = 0
    x_3 = u % p
    z_3 = 1
    swap = 0

    a24 = 121665
    

    for t in range(255 - 1, -1, -1):
        k_t = (k >> t) & 1
        print (str(t) + ":" + hex(x_2))
        swap ^= k_t
        # Conditional swap; see text below.
        (x_2, x_3) = cswap(swap, x_2, x_3)
        (z_2, z_3) = cswap(swap, z_2, z_3)
        swap = k_t
        print (str(t) + ":" + hex(x_2))
       

        A = (x_2 + z_2) 
        AA = (A ** 2) % p 
        B = (x_2 - z_2)
        BB = (B ** 2) % p
        E = (AA - BB) 
        C = (x_3 + z_3) 
        D = (x_3 - z_3) 
        DA = (D * A) 
        CB = (C * B) 
        x_3 = ((DA + CB) ** 2) % p
        z_3 = (x_1 * (DA - CB) ** 2) % p 
        x_2 = (AA * BB) % p
        z_2 = (E * (AA + (a24 * E) )   )  % p
        

    # Conditional swap; see text below.
    (x_2, x_3) = cswap(swap, x_2, x_3)
    (z_2, z_3) = cswap(swap, z_2, z_3)
    return ((x_2 * (pow(z_2, (p - 2), p))) % p)

kBytes = arr.array("B", [   0xa5, 0x46, 0xe3, 0x6b, 0xf0, 0x52, 0x7c, 0x9d, 0x3b, 0x16, 0x15, 0x4b, 0x82, 0x46, 0x5e, 0xdd,
                            0x62, 0x14, 0x4c, 0x0a, 0xc1, 0xfc, 0x5a, 0x18, 0x50, 0x6a, 0x22, 0x44, 0xba, 0x44, 0x9a, 0xc4])

uBytes = arr.array("B", [   0xe6, 0xdb, 0x68, 0x67, 0x58, 0x30, 0x30, 0xdb, 0x35, 0x94, 0xc1, 0xa4, 0x24, 0xb1, 0x5f, 0x7c,
                            0x72, 0x66, 0x24, 0xec, 0x26, 0xb3, 0x35, 0x3b, 0x10, 0xa9, 0x03, 0xa6, 0xd0, 0xab, 0x1c, 0x4c])

def bytes_to_int(bytes):
    result = 0

    for b in bytes:
        result = result * 256 + int(b)

    return result


k = decodeScalar25519(kBytes)
u = decodeUCoordinate(uBytes, 255)

#print (hex(k))
#print (hex(u))
##print (bytes_to_int(kBytes))

result = X25519(k, u)

resultBytes = encodeUCoordinate(result, 255)

#print (hex(result))


resultList = [ord(b) for b in resultBytes]

for x in resultList:
    print ("{:02x}".format(x) , end = '')

print ()

