/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Utils;

/// <summary>
/// Class used to generate a grayscale BMP image from a byte array.
/// </summary>
public class GrayscaleBitmap
{
	/// <summary>
	/// Width of the image in pixels.
	/// </summary>
	public int Width { get; }

	/// <summary>
	/// Height of the image in pixels.
	/// </summary>
	public int Height { get; }

	/// <summary>
	/// Number of bits per pixel.
	/// </summary>
	public int BitsPerPixel => PIXEL_FORMAT;

	/// <summary>
	/// Raw BMP data.
	/// </summary>
	public IReadOnlyList<byte> Data { get; }

	/// <summary>
	/// Size of the BMP header in bytes.
	/// </summary>
	private const int BMP_HEADER_SIZE = 54;

	/// <summary>
	/// Size of the palette in bytes.
	/// </summary>
	private const int PALETTE_SIZE = 1024;

	/// <summary>
	/// Number of bits per pixel.
	/// This is able to be a constant because this class only supports 8-bit
	///   grayscale images.
	/// </summary>
	private const int PIXEL_FORMAT = 8;

	/// <summary>
	/// Size of the Device Independent Bitmap (DIB) header in bytes.
	/// </summary>
	/// <remarks>
	/// A size of 40 corresponds to the Windows BITMAPINFOHEADER format.
	/// </remarks>
	private const int DIB_HEADER_SIZE = 40;

	/// <summary>
	/// Number of planes in the image.
	/// Note that this must always be 1 for bitmaps.
	/// </summary>
	private const int NUMBER_OF_PLANES = 1;

	/// <summary>
	/// Offset in bytes to the size of the BMP file.
	/// </summary>
	private const int FILE_SIZE_OFFSET = 2;

	/// <summary>
	/// Offset in bytes to the offset of the pixel data.
	/// </summary>
	private const int PIXEL_DATA_OFFSET = 10;

	/// <summary>
	/// Offset in bytes to the start of the DIB header.
	/// </summary>
	private const int DIB_HEADER_OFFSET = 14;

	/// <summary>
	/// Offset in bytes to the size of the image data.
	/// </summary>
	private const int IMAGE_DATA_SIZE_OFFSET = 34;

	/// <summary>
	/// Initializes a new instance of the class.
	/// </summary>
	/// <param name="width">Width in pixels of the image.</param>
	/// <param name="height">Height in pixels of the image.</param>
	/// <param name="pixelData">Raw data for the image.</param>
	/// <exception cref="ArgumentException">
	/// Thrown if any argument is set to an invalid value.
	/// </exception>
	public GrayscaleBitmap(int width, int height, byte[] pixelData)
	{
		if (width <= 0 || height <= 0)
		{
			throw new ArgumentException(
				"Width and Height must be greater than 0."
			);
		}

		if (pixelData.Length != width * height)
		{
			throw new ArgumentException(
				"Data array size does not match the dimensions."
			);
		}

		Width = width;
		Height = height;
		using var ms = new MemoryStream();

		// Helper method for ensuring that the BMP data is written in the
		//   correct endianness
		// Note that this also means that 2-byte values can also be written
		//   by using this method with an int since the upper 2 bytes will
		//   be ignored
		var toBytes = BitConverter.IsLittleEndian
			? BitConverter.GetBytes
			: (Func<int, byte[]>)(i =>
				BitConverter.GetBytes(i).Reverse().ToArray()
			);

		// Calculate the stride of the image
		// Note that for the .bmp format, the stride must be a multiple of 4
		var stride = (int)Math.Ceiling((double)(Width * BitsPerPixel) / 32) * 4;

		// Calculate the total file size
		var imageSize = stride * Height;

		// Write the BMP header
		// Skip writing the file size for now - this will be handled at the end
		var header = new byte[BMP_HEADER_SIZE];
		header[0] = 0x42; // B
		header[1] = 0x4D; // M

		// Offset to pixel data
		var offset = BMP_HEADER_SIZE + PALETTE_SIZE;
		Array.Copy(
			toBytes(offset),
			0,
			header,
			PIXEL_DATA_OFFSET,
			sizeof(int)
		);

		// Write the DIB header to the image buffer
		Array.Copy(
			toBytes(DIB_HEADER_SIZE),
			0,
			header,
			DIB_HEADER_OFFSET,
			sizeof(int)
		);
		Array.Copy(
			toBytes(Width),
			0,
			header,
			DIB_HEADER_OFFSET + (1 * sizeof(int)),
			sizeof(int)
		);
		Array.Copy(
			toBytes(Height),
			0,
			header,
			DIB_HEADER_OFFSET + (2 * sizeof(int)),
			sizeof(int)
		);
		Array.Copy(
			toBytes(NUMBER_OF_PLANES),
			0,
			header,
			DIB_HEADER_OFFSET + (3 * sizeof(int)),
			sizeof(short)
		);
		Array.Copy(
			toBytes(PIXEL_FORMAT),
			0,
			header,
			DIB_HEADER_OFFSET + (3 * sizeof(int)) + (1 * sizeof(short)),
			sizeof(short)
		);

		// Image data size
		Array.Copy(
			toBytes(stride * Height),
			0,
			header,
			IMAGE_DATA_SIZE_OFFSET,
			sizeof(int)
		);

		// Write the header to the image buffer
		ms.Write(header, 0, header.Length);

		// Write the grayscale palette
		for (var i = 0; i < 256; i++)
		{
			ms.WriteByte((byte)i);
			ms.WriteByte((byte)i);
			ms.WriteByte((byte)i);

			// No pixels should be transparent
			ms.WriteByte(0);
		}

		// Write image data
		// Note that the .bmp format stores data from bottom to top
		for (var y = Height - 1; y >= 0; y--)
		{
			for (var x = 0; x < Width; x++)
			{
				ms.WriteByte(pixelData[(y * Width) + x]);
			}

			// Write padding bytes for the row
			for (var i = 0; i < stride - Width; i++)
			{
				ms.WriteByte(0);
			}
		}

		// Finalize the image data by writing the file size to the buffer
		var bmpData = ms.ToArray();
		Array.Copy(
			toBytes(bmpData.Length),
			0,
			bmpData,
			FILE_SIZE_OFFSET,
			sizeof(int)
		);
		Data = bmpData;
	}

	/// <summary>
	/// Converts the BMP data to a base64 string.
	/// </summary>
	/// <returns>The BMP data as a base64 string.</returns>
	public string ToBase64String()
	{
		// Convert the BMP data to base64
		return Convert.ToBase64String(Data.ToArray());
	}
}
