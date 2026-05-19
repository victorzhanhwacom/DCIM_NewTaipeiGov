using UnityEditor;

/// <summary>
/// Configures PNG textures as Sprites ONLY when they are first added to the project.
/// This will not interfere if you manually change them to Default (for Materials) or Multiple later.
/// </summary>
public class TexturePostProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;

        // assetImporter.importSettingsMissing is true only for brand new files
        // being dragged into the Project window for the first time.
        if (textureImporter.importSettingsMissing)
        {
            if (assetPath.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
            {
                // Initialize as Sprite (2D and UI)
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                
                // UI & Performance Best Practices
                textureImporter.alphaIsTransparency = true;
                textureImporter.mipmapEnabled = false;
            }
        }
    }
}